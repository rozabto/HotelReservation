using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, string>
    {
        private readonly IReservationRepository _reservation;
        private readonly IHotelRoomRepository _hotelRoom;

        public CreateReservationHandler(IReservationRepository reservation, IHotelRoomRepository hotelRoom)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
        }

        public async Task<string> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            request.To = request.To.Date;
            request.From = request.From.Date;

            var room = await _hotelRoom.GetById(request.RoomId, cancellationToken)
                ?? throw new ModelStateException(Array.Empty<string>(), "Room Not Found");

            if (!room.RoomPrice.HasValue)
            {
                if (!request.Adults.HasValue || !request.Children.HasValue)
                {
                    throw new ModelStateException(new[] { nameof(request.Adults), nameof(request.Children) },
                        "Adults and children count must have a value");
                }

                if (room.Capacity < request.Adults + request.Children)
                {
                    throw new ModelStateException(new[] { nameof(request.Adults), nameof(request.Children) },
                        "Room can only store up to " + room.Capacity + " people");
                }
            }

            if (await _reservation.CanReserve(request.RoomId, request.To, request.From, cancellationToken))
                throw new ModelStateException(Array.Empty<string>(), "You can't reserve already reserved rooms");

            var reservation = new Reservation
            {
                AllInclusive = request.Include == ReservationInclude.All,
                IncludeFood = request.Include == ReservationInclude.Food,
                Price = (decimal)(request.To - request.From).TotalDays
                * (room.RoomPrice ?? room.PriceForAdults.Value * request.Adults.Value
                    + room.PriceForChildren.Value + request.Children.Value)
                + (request.Include == ReservationInclude.All ? (50m + room.FoodPrice) : 0m)
                + (request.Include == ReservationInclude.Food ? room.FoodPrice : 0m),
                ReservedForDate = request.From,
                ReservedUntilDate = request.To,
                ReservedRoomId = request.RoomId,
                TransactionId = GlobalVariables.IsEnvironmentProduction ? null : (ulong?)0
            };

            await _reservation.Create(reservation, cancellationToken);

            return reservation.Id;
        }
    }
}