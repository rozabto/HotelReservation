using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand>
    {
        public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
