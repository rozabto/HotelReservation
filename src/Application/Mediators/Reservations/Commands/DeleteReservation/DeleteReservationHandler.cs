using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Reservations.Commands.DeleteReservation
{
    public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand>
    {
        private readonly IReservationRepository _reservation;
        private readonly ICurrentUserService _currentUser;
        private readonly ICheckoutService _checkout;
        private readonly IConfiguration _configuration;
        private readonly IDateTime _date;

        public DeleteReservationHandler(IReservationRepository reservation, ICurrentUserService currentUser, ICheckoutService checkout, IConfiguration configuration, IDateTime date)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<Unit> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservation.FindByRoomId(request.RoomId, _currentUser.User.Id, cancellationToken)
                ?? throw new NotFoundException("Reservation", request.RoomId);

            if (reservation.TransactionId.HasValue && reservation.TransactionId.Value != 0)
                await RefundReservation(reservation);

            reservation.DeletedOn = _date.Now;

            await _reservation.Update(reservation, cancellationToken);

            return Unit.Value;
        }

        private async Task RefundReservation(Reservation reservation)
        {
            var priceStr = reservation.Price.ToString().Replace(',', '.');
            var price = priceStr.Contains('.') ? priceStr : priceStr + ".00";

            var parameters = new Dictionary<string, string>
            {
                { "merchantId", _configuration.GetValue<string>("Key:SafeCharge:Merchant") },
                { "merchantSiteId", _configuration.GetValue<string>("Key:SafeCharge:Site") },
                { "clientRequestId", DateTime.UtcNow.Ticks.ToString() },
                { "clientUniqueId", _currentUser.User.Email },
                { "amount", price },
                { "currency", "EUR" },
                { "relatedTransactionId", reservation.TransactionId.Value.ToString() },
                { "authCode", reservation.AuthCode ?? string.Empty },
                { "timeStamp", DateTime.UtcNow.ToString("yyyyMMddHHmmss") }
            };

            parameters.Add("checksum", _checkout.CalculateChecksum(parameters, _configuration.GetValue<string>("Key:SafeCharge:Secret"), false));

            using var http = new HttpClient();

            var response = await http.PostAsync("https://ppp-test.safecharge.com/ppp/api/v1/refundTransaction.do",
                new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));

            var data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()) as dynamic;

            if (data.status != "SUCCESS")
                throw new BadRequestException("Couldn't refund the reservation with id " + reservation.Id);
        }
    }
}
