using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Data.Commands.ExpiredReservations
{
    public class ExpiredReservationHandler : IRequestHandler<ExpiredReservationsCommand>
    {
        private readonly IReservationRepository _reservation;
        private readonly IDateTime _date;

        public ExpiredReservationHandler(IReservationRepository reservation, IDateTime date)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<Unit> Handle(ExpiredReservationsCommand request, CancellationToken cancellationToken)
        {
            await _reservation.DeleteExpired(_date.Now, cancellationToken);
            return Unit.Value;
        }
    }
}
