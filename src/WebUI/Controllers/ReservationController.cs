using System;
using System.Threading.Tasks;
using Application.Reservations.Commands.CompleteCheckout;
using Application.Reservations.Commands.CreateReservation;
using Application.Reservations.Queries.Checkout;
using Application.Reservations.Queries.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class ReservationController : BaseController
    {
        public async Task<IActionResult> Index(int? page) =>
            View(await Mediator.Send(new ReservationQuery { Page = page ?? 0 }));

        public async Task<IActionResult> Checkout(string id, DateTime from, DateTime to, bool includeFood, bool allInclusive) =>
            View(await Mediator.Send(new CheckoutQuery
            {
                AllInclusive = allInclusive,
                From = from,
                IncludeFood = includeFood,
                RoomId = id,
                To = to
            }));

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateReservationCommand command)
        {
            await Mediator.Send(command);
            return Redirect(nameof(Index));
        }

        public async Task<IActionResult> Complete(string id) =>
            await Mediator.Send(new CompleteCheckoutCommand { Id = id })
                ? Redirect(nameof(Index)) : (IActionResult)View("Failed");
    }
}
