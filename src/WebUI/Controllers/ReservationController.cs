using System.Threading.Tasks;
using Application.Reservations.Queries.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class ReservationController : BaseController
    {
        public async Task<IActionResult> Checkout() =>
            View(await Mediator.Send(new CheckoutQuery()));
    }
}
