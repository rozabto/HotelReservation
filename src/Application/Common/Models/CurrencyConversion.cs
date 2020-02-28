using System.Collections.Generic;

namespace Application.Common.Models
{
    public class CurrencyConversion
    {
        public IReadOnlyDictionary<string, decimal> Rates { get; set; }
    }
}
