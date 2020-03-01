using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, string>
    {
        private readonly IReservationRepository _reservation;
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly ICurrentUserService _currentUser;
        private readonly ICountryService _country;
        private readonly ITimeZoneService _timeZone;

        public CreateReservationHandler(IReservationRepository reservation, IHotelRoomRepository hotelRoom, ICurrentUserService currentUser, ICountryService country, ITimeZoneService timeZone)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _country = country ?? throw new ArgumentNullException(nameof(country));
            _timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        }

        public async Task<string> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            {
                var countryCode = await _country.GetCountryCode(_currentUser.Ip);

                request.To = _timeZone.ConvertDateFromCountryCode(countryCode, request.To).Date;
                request.From = _timeZone.ConvertDateFromCountryCode(countryCode, request.From).Date;
            }

            var room = await _hotelRoom.GetById(request.RoomId, cancellationToken)
                ?? throw new NotFoundException("Room Id", request.RoomId);

            if (!room.RoomPrice.HasValue)
            {
                if (!request.Adults.HasValue || !request.Children.HasValue)
                    throw new BadRequestException("Adults and children count must have a value");

                if (room.Capacity < request.Adults + request.Children)
                    throw new BadRequestException("Room can only store up to " + room.Capacity + " people");
            }

            if (await _reservation.CanReserve(request.RoomId, request.To, request.From, cancellationToken))
                throw new BadRequestException("You can't reserve already reserved rooms");

            var reservation = new Reservation
            {
                AllInclusive = request.AllInclusive,
                IncludeFood = request.IncludeFood,
                Price = (decimal)(request.To - request.From).TotalDays
                * (room.RoomPrice ?? room.PriceForAdults.Value * request.Adults.Value
                    + room.PriceForChildren.Value + request.Children.Value)
                + (request.AllInclusive ? 50m : 0m)
                + (request.IncludeFood ? room.FoodPrice : 0m),
                ReservedForDate = request.From,
                ReservedUntilDate = request.To,
                ReservedRoomId = request.RoomId,
                TransactionId = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ? null : (ulong?)0
            };

            await _reservation.Create(reservation, cancellationToken);

            return reservation.Id;
        }
    }
}