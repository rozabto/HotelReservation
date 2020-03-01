using System.Threading.Tasks;
using Application.Home.Queries.GetHotelRoom;
using Application.Reservations.Commands.CreateReservation;
using Application.Reservations.Commands.DeleteReservation;
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

        public async Task<IActionResult> Create(string id)
        {
            ViewData["Room"] = await Mediator.Send(new GetHotelRoomQuery { Id = id });
            return View();
        }

        public async Task<IActionResult> Checkout(string id) =>
            View(await Mediator.Send(new CheckoutQuery { Id = id }));

        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteReservationCommand { RoomId = id });
            return Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Room"] = await Mediator.Send(new GetHotelRoomQuery { Id = command.RoomId });
                return View();
            }

            var id = await Mediator.Send(command);
            return Redirect("/Reservation/" + nameof(Checkout) + '/' + id);
        }
    }
}