using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICheckoutService
    {
        string GenerateCheckout(Dictionary<string, string> parameters, string merchantKey);
        string CalculateChecksum(Dictionary<string, string> parameters, string merchantKey, bool firstKey = true);
        string GetHash(string text);
    }
}
