using System;
using MediatR;

namespace Application.Reservations.Queries.Checkout
{
    public class CheckoutQuery : IRequest<CheckoutResponse>
    {
        public string RoomId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IncludeFood { get; set; }
        public bool AllInclusive { get; set; }
    }
}
