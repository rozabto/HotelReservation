using MediatR;

namespace Application.Reservations.Commands.CompleteCheckout
{
    public class CompleteCheckoutCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
