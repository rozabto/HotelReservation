using Application.Common.Exceptions;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand>
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

        public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var room = await _hotelRoom.GetById(request.RoomId, cancellationToken)
                ?? throw new NotFoundException("Room Id", request.RoomId);

            if (await _reservation.CanReserve(request.RoomId, _currentUser.User.Id, request.To, request.From, cancellationToken))
                throw new BadRequestException("You can't reserve already reserved rooms");

            await _reservation.Create(new Reservation
            {
                AllInclusive = request.AllInclusive,
                HasCompleted = true,
                IncludeFood = request.IncludeFood,
                Price = (decimal)(request.To - request.From).TotalDays
                * (room.PriceForAdults ?? room.RoomPrice).Value
                + (request.AllInclusive ? 50m : 0m)
                + (request.IncludeFood ? room.FoodPrice : 0m),
                ReservedForDate = request.From,
                ReservedUntilDate = request.To,
                ReservedRoomId = request.RoomId
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
