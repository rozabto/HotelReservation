using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Reservations.Commands.CompleteReservation
{
    public class CompleteReservationHandler : IRequestHandler<CompleteReservationCommand>
    {
        private readonly ICheckoutService _checkout;
        private readonly IConfiguration _configuration;
        private readonly IReservationRepository _reservation;
        private readonly IUserManager _userManager;

        public CompleteReservationHandler(ICheckoutService checkout, IConfiguration configuration, IReservationRepository reservation, IUserManager userManager)
        {
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(CompleteReservationCommand request, CancellationToken cancellationToken)
        {
            var str = _configuration.GetValue<string>("Key:SafeCharge:Secret") + request.TotalAmount
                + request.Currency + request.ResponseTimeStamp + request.PPP_TransactionID
                + request.Status + request.ProductId;

            if (_checkout.GetHash(str) == request.AdvanceResponseChecksum)
            {
                var user = await _userManager.GetUserByEmail(request.Email);
                var reservation = await _reservation.FindByRoomId(request.ProductId, user.Id, cancellationToken);
                reservation.HasCompleted = true;
                await _reservation.Update(reservation, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
