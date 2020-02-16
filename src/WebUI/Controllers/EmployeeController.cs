using System.Threading.Tasks;
using Application.Employees.Commands.CreateHotelRoom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateHotelRoomCommand command)
        {
            if (!ModelState.IsValid)
                return View();

            await Mediator.Send(command);
            return Redirect("/");
        }
    }
}
