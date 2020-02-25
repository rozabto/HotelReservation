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
            string email = default)
        {
            if (ppp_status == "OK")
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
                    Email = email
                });
            }
            return Ok();
        }
    }
}
