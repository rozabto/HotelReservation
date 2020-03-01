using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using NodaTime.TimeZones;

namespace Infrastructure.Common
{
    public class TimeZoneService : ITimeZoneService
    {
        public DateTime ConvertDateFromCountryCode(string countryCode, DateTime date)
        {
            var source = TzdbDateTimeZoneSource.Default;
            var zoneId = source.ZoneLocations
                .Where(x => x.CountryCode == countryCode)
                .Select(tz => source.WindowsMapping.MapZones
                    .FirstOrDefault(x => x.TzdbIds.Contains(
                                         source.CanonicalIdMap.First(y => y.Value == tz.ZoneId).Key)))
                .Where(x => x != null)
                .Select(x => x.WindowsId)
                .Distinct()
                .FirstOrDefault();
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);

            return TimeZoneInfo.ConvertTimeToUtc(date, easternZone);
        }
    }
}
