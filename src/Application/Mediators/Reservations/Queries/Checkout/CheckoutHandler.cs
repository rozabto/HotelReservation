using System;
using System.Collections.Generic;
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

            if (await _reservation.CanReserve(request.RoomId, _currentUser.User.Id, request.To, request.From, cancellationToken))
                throw new BadRequestException("You can't reserve already reserved rooms");

            var merchantKey = _configuration.GetValue<string>("Key:SafeCharge");
            var merchantId = _configuration.GetValue<string>("Key:SafeChargeMId");

            var price = (decimal)(request.To - request.From).TotalDays
                * (room.PriceForAdults ?? room.RoomPrice).Value
                + (request.AllInclusive ? 50m : 0m)
                + (request.IncludeFood ? room.FoodPrice : 0m);

            if (!await _reservation.CheckIfExists(request.RoomId, _currentUser.User.Id, cancellationToken))
            {
                await _reservation.Create(new Reservation
                {
                    AllInclusive = request.AllInclusive,
                    IncludeFood = request.IncludeFood,
                    Price = price,
                    ReservedForDate = request.From,
                    ReservedRoomId = room.Id,
                    ReservedUntilDate = request.To
                }, cancellationToken);
            }

            var priceStr = price.ToString().Replace(',', '.');

            var _params = ConstructParametersMap(room, merchantId,
                priceStr.Contains('.') ? priceStr : priceStr + ".00");

            return new CheckoutResponse
            {
                Url = _checkout.GenerateCheckout(_params, merchantKey)
            };
        }

        private Dictionary<string, string> ConstructParametersMap(HotelRoom room, string merchantId, string price) =>
            new Dictionary<string, string>
            {
                { "currency", "EUR" },
                { "item_name_1", room.Id },
                { "item_number_1", "1" },
                { "item_quantity_1", "1" },
                { "item_amount_1", price },
                { "numberofitems", "1" },
                { "encoding", "utf-8" },
                { "merchant_id", merchantId },
                { "merchant_site_id", "183073" },
                { "time_stamp", _date.Now.ToString("yyyy-MM-dd.HH:mm:ss") },
                { "version", "4.0.0" },
                { "user_token_id", _currentUser.User.Email },
                { "user_token", "auto" },
                { "total_amount", price },
                { "notify_url", "https://sandbox.safecharge.com/lib/demo_process_request/response.php" }
            };
    }
}
