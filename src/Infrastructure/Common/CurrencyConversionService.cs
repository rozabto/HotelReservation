using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Newtonsoft.Json;

namespace Infrastructure.Common
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private IReadOnlyDictionary<string, decimal> currencies;

        public decimal ConvertFromCountryCode(string currencyCode) =>
            currencies.TryGetValue(currencyCode, out var value) ? value : 1m;

        public async Task GetLatestCountryCodes()
        {
            string response;

            using (var http = new HttpClient())
                response = await http.GetStringAsync("https://api.exchangeratesapi.io/latest");

            currencies = JsonConvert.DeserializeObject<CurrencyConversion>(response).Rates;
        }
    }
}
