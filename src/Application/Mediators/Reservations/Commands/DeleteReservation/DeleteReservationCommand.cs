using MediatR;

namespace Application.Reservations.Commands.DeleteReservation
{
    public class DeleteReservationCommand : IRequest
    {
        public string RoomId { get; set; }
    }
}
