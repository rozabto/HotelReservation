using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICurrencyConversionService
    {
        decimal ConvertFromCountryCode(string countryCode);

        Task GetLatestCountryCodes();
    }
}
