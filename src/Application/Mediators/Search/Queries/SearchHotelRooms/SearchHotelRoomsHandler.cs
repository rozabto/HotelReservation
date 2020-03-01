using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsHandler : IRequestHandler<SearchHotelRoomsQuery, SearchHotelRoomsResponse>
    {
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly ICountryService _country;
        private readonly ICurrentUserService _currentUser;
        private readonly ICurrencyConversionService _currencyConversion;
        private readonly ITimeZoneService _timeZone;

        public SearchHotelRoomsHandler(IHotelRoomRepository hotelRoom, ICountryService country, ICurrentUserService currentUser, ICurrencyConversionService currencyConversion, ITimeZoneService timeZone)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _country = country ?? throw new ArgumentNullException(nameof(country));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _currencyConversion = currencyConversion ?? throw new ArgumentNullException(nameof(currencyConversion));
            _timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        }

        public async Task<SearchHotelRoomsResponse> Handle(SearchHotelRoomsQuery request, CancellationToken cancellationToken)
        {
            var countryCode = await _country.GetCountryCode(_currentUser.Ip);
            var currencyCode = new RegionInfo(countryCode).ISOCurrencySymbol;
            var currency = _currencyConversion.ConvertFromCountryCode(currencyCode);

            if (request.Start.HasValue)
                request.Start = Math.Round(request.Start.Value / currency);

            if (request.End.HasValue)
                request.End = Math.Round(request.End.Value / currency);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            {
                if (request.AvailableFrom.HasValue)
                    request.AvailableFrom = _timeZone.ConvertDateFromCountryCode(countryCode, request.AvailableFrom.Value).Date;

                if (request.AvailableTo.HasValue)
                    request.AvailableTo = _timeZone.ConvertDateFromCountryCode(countryCode, request.AvailableTo.Value).Date;
            }

            var count = await _hotelRoom.SearchedHotelRoomsCount(
                request.Term,
                request.AvailableFrom,
                request.AvailableTo,
                request.Capacity,
                request.RoomType,
                request.Start,
                request.End,
                cancellationToken
            );

            return new SearchHotelRoomsResponse
            {
                HotelRooms = await _hotelRoom.SearchHotelRooms(
                        request.Term,
                        currency,
                        request.AvailableFrom,
                        request.AvailableTo,
                        request.Capacity,
                        request.Page,
                        20,
                        request.RoomType,
                        request.Start,
                        request.End,
                        request.SortBy,
                        cancellationToken
                    ),
                Count = count,
                HighestPrice = count > 0 ? (int)(Math.Ceiling(await _hotelRoom.HighestPricesRoomSearch(
                        request.Term,
                        request.AvailableFrom,
                        request.AvailableTo,
                        request.Capacity,
                        request.RoomType,
                        request.Start,
                        request.End,
                        cancellationToken
                    ) * currency)) : 0,
                CurrencyCode = currencyCode,
            };
        }
    }
}