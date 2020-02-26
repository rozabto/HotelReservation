using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
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

        public CreateReservationHandler(IReservationRepository reservation, IHotelRoomRepository hotelRoom, ICurrentUserService currentUser)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<string> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            request.To = request.To.Date;
            request.From = request.From.Date;

            if (request.From > request.To)
                (request.To, request.From) = (request.From, request.To);

            var room = await _hotelRoom.GetById(request.RoomId, cancellationToken)
                ?? throw new NotFoundException("Room Id", request.RoomId);

            if (!room.RoomPrice.HasValue && (!request.Adults.HasValue || !request.Children.HasValue))
                throw new BadRequestException("Adults and children count must have a value");

            if (await _reservation.CanReserve(request.RoomId, _currentUser.User.Id, request.To, request.From, cancellationToken))
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
                TransactionId = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? (ulong?)0 : null
            };

            await _reservation.Create(reservation, cancellationToken);

            return reservation.Id;
        }
    }
}