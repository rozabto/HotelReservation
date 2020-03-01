using System;

namespace Application.Common.Interfaces
{
    public interface ITimeZoneService
    {
        DateTime ConvertDateFromCountryCode(string countryCode, DateTime date);
    }
}
