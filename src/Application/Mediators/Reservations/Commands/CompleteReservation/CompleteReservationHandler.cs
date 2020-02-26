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

        public CompleteReservationHandler(ICheckoutService checkout, IConfiguration configuration, IReservationRepository reservation)
        {
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
        }

        public async Task<Unit> Handle(CompleteReservationCommand request, CancellationToken cancellationToken)
        {
            var str = _configuration.GetValue<string>("Key:SafeCharge:Secret") + request.TotalAmount
                + request.Currency + request.ResponseTimeStamp + request.PPP_TransactionID
                + request.Status + request.ProductId;

            if (_checkout.GetHash(str) == request.AdvanceResponseChecksum)
            {
                var reservation = await _reservation.FindByRoomId(request.ProductId, request.UserId, cancellationToken);
                reservation.TransactionId = request.TransactionId;
                reservation.AuthCode = request.AuthCode;
                await _reservation.Update(reservation, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
