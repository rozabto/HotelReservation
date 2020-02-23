using MediatR;
using System;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommand : IRequest
    {
        public string RoomId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IncludeFood { get; set; }
        public bool AllInclusive { get; set; }
        public int Children { get; set; }
        public int Adults { get; set; }
    }
}
