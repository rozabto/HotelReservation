using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICheckoutService
    {
        string GenerateCheckout(Dictionary<string, string> parameters, string merchantKey);
        string CalculateChecksum(Dictionary<string, string> _params, string merchantKey);
    }
}
