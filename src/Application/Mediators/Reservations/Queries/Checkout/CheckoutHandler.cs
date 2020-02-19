using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Reservations.Queries.Checkout
{
    public class CheckoutHandler : IRequestHandler<CheckoutQuery, CheckoutResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly IReservationRepository _reservation;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _date;
        private readonly ICheckoutService _checkout;

        public CheckoutHandler(IConfiguration configuration, IHotelRoomRepository hotelRoom, IReservationRepository reservation, ICurrentUserService currentUser, IDateTime date, ICheckoutService checkout)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
        }

        public async Task<CheckoutResponse> Handle(CheckoutQuery request, CancellationToken cancellationToken)
        {
            var room = await _hotelRoom.GetById(request.RoomId, cancellationToken)
                ?? throw new NotFoundException("Room", request.RoomId);

            var marchantKey = _configuration.GetValue<string>("Key:SafeCharge");

            SessionTokenJson sessionToken;

            using (var http = new HttpClient())
            {
                var identifier = _date.Now.Ticks.ToString();

                var parameters = ConstructGetSessionToken(identifier);
                var checksum = _checkout.CalculateChecksum(parameters, marchantKey);
                parameters.Add("checksum", checksum);

                var response = await http.PostAsync(
                    "https://ppp-test.safecharge.com/ppp/api/v1/getSessionToken.do",
                    new FormUrlEncodedContent(parameters)
                );

                sessionToken = JsonConvert.DeserializeObject<SessionTokenJson>(await response.Content.ReadAsStringAsync());
            }

            var price = (decimal)(request.To - request.From).TotalDays
                * (room.PriceForAdults ?? room.RoomPrice).Value
                + (request.AllInclusive ? 100m : 0m)
                + (request.IncludeFood ? room.FoodPrice : 0m);

            var _params = ConstructParametersMap(room, price.ToString());

            return new CheckoutResponse
            {
                Url = _checkout.GenerateCheckout(_params, marchantKey)
            };
        }

        private Dictionary<string, string> ConstructParametersMap(HotelRoom room, string price) =>
            new Dictionary<string, string>
            {
                { "currency", "EUR" },
                { "item_name_1", room.Name },
                { "item_number_1", "1" },
                { "item_quantity_1", "1" },
                { "item_amount_1", price },
                { "numberofitems", "1" },
                { "encoding", "utf-8" },
                { "merchant_id", "4778151621448449994" },
                { "merchant_site_id", "183073" },
                { "time_stamp", "2019-07-08.09:55:50" },
                { "version", "4.0.0" },
                { "user_token_id", _currentUser.User.Email },
                { "user_token", "auto" },
                { "total_amount", price },
                { "notify_url", "https://localhost:5001" }
            };

        private Dictionary<string, string> ConstructGetSessionToken(string identifier) =>
            new Dictionary<string, string>
            {
                { "merchantId", "4778151621448449994" },
                { "merchantSiteId", "183073" },
                { "clientRequestId", identifier },
                { "timeStamp", "20170118191622" }
            };
    }
}
