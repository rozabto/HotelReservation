using MediatR;

namespace Application.Reservations.Queries.Checkout
{
    public class CheckoutQuery : IRequest<CheckoutResponse>
    {
        public string Id { get; set; }
    }
}
