using System.Threading.Tasks;
using Application.Reservations.Commands.CreateReservation;
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

        public IActionResult Create(string roomId, bool normalRoom)
        {
            ViewData["RoomId"] = roomId;
            ViewData["NormalRoom"] = normalRoom;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateReservationCommand command)
        {
            await Mediator.Send(command);
            return Redirect(nameof(Index));
        }
    }
}