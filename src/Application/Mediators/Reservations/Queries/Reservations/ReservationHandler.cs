using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Reservations.Queries.Reservations
{
    public class ReservationHandler : IRequestHandler<ReservationQuery, ReservationsResponse>
    {
        private readonly IReservationRepository _reservation;
        private readonly ICurrentUserService _currentUser;

        public ReservationHandler(IReservationRepository reservation, ICurrentUserService currentUser)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<ReservationsResponse> Handle(ReservationQuery request, CancellationToken cancellationToken) =>
            new ReservationsResponse
            {
                Reservations = await _reservation.GetUserReservations(_currentUser.User.Id, request.Page, 20, cancellationToken)
            };
    }
}