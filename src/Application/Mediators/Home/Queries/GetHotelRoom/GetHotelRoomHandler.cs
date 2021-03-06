﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Home.Queries.GetHotelRoom
{
    public class GetHotelRoomHandler : IRequestHandler<GetHotelRoomQuery, GetHotelRoomResponse>
    {
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly ICountryService _country;
        private readonly ICurrencyConversionService _currencyConversion;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _date;

        public GetHotelRoomHandler(IHotelRoomRepository hotelRoom, ICountryService country, ICurrencyConversionService currencyConversion, ICurrentUserService currentUser, IDateTime date)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _country = country ?? throw new ArgumentNullException(nameof(country));
            _currencyConversion = currencyConversion ?? throw new ArgumentNullException(nameof(currencyConversion));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<GetHotelRoomResponse> Handle(GetHotelRoomQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id) || request.Id.Length != 32)
                throw new NotFoundException("Room", request.Id);

            var countryCode = await _country.GetCountryCode(_currentUser.Ip);
            var currencyCode = new RegionInfo(countryCode).ISOCurrencySymbol;
            var conversionRate = _currencyConversion.ConvertFromCountryCode(currencyCode);

            var room = await _hotelRoom.GetVmById(request.Id, conversionRate, _date.Now.AddMonths(-3).Date, cancellationToken)
                ?? throw new NotFoundException("Room", request.Id);

            return new GetHotelRoomResponse
            {
                Room = room,
                CurrencyCode = currencyCode,
                ConversionRate = conversionRate
            };
        }
    }
}