using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Reservations.Queries.Checkout
{
    public class CheckoutHandler : IRequestHandler<CheckoutQuery, CheckoutResponse>
    {
        private readonly IReservationRepository _reservation;
        private readonly ICountryService _country;
        private readonly ICurrencyConversionService _currencyConversion;
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration _configuration;
        private readonly IDateTime _date;
        private readonly ICheckoutService _checkout;

        public CheckoutHandler(IReservationRepository reservation, ICountryService country, ICurrencyConversionService currencyConversion, ICurrentUserService currentUser, IConfiguration configuration, IDateTime date, ICheckoutService checkout)
        {
            _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
            _country = country ?? throw new ArgumentNullException(nameof(country));
            _currencyConversion = currencyConversion ?? throw new ArgumentNullException(nameof(currencyConversion));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _checkout = checkout ?? throw new ArgumentNullException(nameof(checkout));
        }

        public async Task<CheckoutResponse> Handle(CheckoutQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservation.GetById(request.Id, cancellationToken)
                ?? throw new NotFoundException("Reservation Id", request.Id);

            if (reservation.CreatedById != _currentUser.User.Id)
                throw new BadRequestException("This reservation is not yours");

            var countryCode = await _country.GetCountryCode(_currentUser.Ip);
            var currencyCode = new RegionInfo(countryCode).ISOCurrencySymbol;
            var conversionRate = _currencyConversion.ConvertFromCountryCode(currencyCode);

            var _params = ConstructParametersMap(reservation, currencyCode, conversionRate);

            return new CheckoutResponse
            {
                Url = _checkout.GenerateCheckout(_params, _configuration.GetValue<string>("Key:SafeCharge:Secret"))
            };
        }

        private Dictionary<string, string> ConstructParametersMap(Reservation reservation, string currencyCode, decimal conversionRate)
        {
            var priceStr = Math.Round(reservation.Price * conversionRate, 2).ToString().Replace(',', '.');
            var price = priceStr.Contains('.') ? priceStr : priceStr + ".00";

            return new Dictionary<string, string>
            {
                { "currency", currencyCode },
                { "item_name_1", reservation.ReservedRoomId },
                { "item_number_1", "1" },
                { "item_quantity_1", "1" },
                { "item_amount_1", price },
                { "numberofitems", "1" },
                { "encoding", "utf-8" },
                { "merchant_id", _configuration.GetValue<string>("Key:SafeCharge:Merchant") },
                { "merchant_site_id", _configuration.GetValue<string>("Key:SafeCharge:Site") },
                { "time_stamp", _date.Now.ToString("yyyy-MM-dd.HH:mm:ss") },
                { "version", "4.0.0" },
                { "user_token_id", _currentUser.User.Email },
                { "user_token", "auto" },
                { "total_amount", price },
                { "userId", _currentUser.User.Id },
                { "notify_url", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                    ? "https://hotel-reservation-manager.herokuapp.com/api/safecharge"
                    : "https://sandbox.safecharge.com/lib/demo_process_request/response.php" }
            };
        }
    }
}
