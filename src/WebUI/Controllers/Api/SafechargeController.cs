using System.Threading.Tasks;
using Application.Reservations.Commands.CompleteReservation;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Api
{
    public class SafechargeController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetSafechargeResponse(
            string ppp_status = default,
            string totalAmount = default,
            string Currency = default,
            string responseTimeStamp = default,
            string PPP_TransactionID = default,
            string Status = default,
            string productId = default,
            string advanceResponseChecksum = default,
            string unknownParameters = default,
            ulong? TransactionID = default,
            string authCode = default)
        {
            if (ppp_status == "OK"
                && !string.IsNullOrWhiteSpace(totalAmount)
                && !string.IsNullOrWhiteSpace(Currency)
                && !string.IsNullOrWhiteSpace(responseTimeStamp)
                && !string.IsNullOrWhiteSpace(PPP_TransactionID)
                && !string.IsNullOrWhiteSpace(Status)
                && !string.IsNullOrWhiteSpace(productId)
                && !string.IsNullOrWhiteSpace(advanceResponseChecksum)
                && !string.IsNullOrWhiteSpace(unknownParameters)
                && TransactionID != default)
            {
                await Mediator.Send(new CompleteReservationCommand
                {
                    Currency = Currency,
                    PPP_TransactionID = PPP_TransactionID,
                    ProductId = productId,
                    ResponseTimeStamp = responseTimeStamp,
                    Status = Status,
                    TotalAmount = totalAmount,
                    AdvanceResponseChecksum = advanceResponseChecksum,
                    UserId = unknownParameters.Split('=')[1],
                    TransactionId = TransactionID.Value,
                    AuthCode = authCode
                });
            }
            return Ok();
        }
    }
}
