using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Reservations.Commands.CompleteCheckout
{
    public class CompleteCheckoutHandler : IRequestHandler<CompleteCheckoutCommand, bool>
    {
        private readonly ICheckoutService _checkout;
        private readonly IReservationRepository _reservation;
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration _configuration;
        private readonly IDateTime _date;

        public CompleteCheckoutHandler(ICheckoutService checkout, IReservationRepository reservation, ICurrentUserService currentUser, IConfiguration configuration, IDateTime date)
        {
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<bool> Handle(CompleteCheckoutCommand request, CancellationToken cancellationToken)
        {
            using (var http = new HttpClient())
            {
                var parameters = CreateParamteres();
                parameters.Add("checksum", _checkout.CalculateChecksum(parameters,
                    _configuration.GetValue<string>("Key:SafeCharge"), false));
                parameters.Add("wdRequestId", request.Id);

                var res = await http.PostAsync("https://ppp-test.safecharge.com/ppp/api/withdrawal/getOrders.do",
                    new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));

                var data = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync());
            }
            return true;
        }

        private Dictionary<string, string> CreateParamteres() =>
            new Dictionary<string, string>
            {
                { "merchantId", _configuration.GetValue<string>("Key:SafeChargeMId") },
                { "merchantSiteId", "183073" },
                { "merchantUniqueId", _currentUser.User.Email },
                { "timeStamp", _date.Now.ToString("yyyyMMddHHssmm") }
            };
    }
}
