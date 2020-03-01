using System.Threading.Tasks;
using Application.Employees.Commands.CreateHotelRoom;
using Application.Employees.Commands.DeleteHotelRoom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeeController : BaseController
    {
        public IActionResult Create() => View();

        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteHotelRoomCommand { Id = id });
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHotelRoomCommand command)
        {
            if (!ModelState.IsValid)
                return View();

            await Mediator.Send(command);
            return Redirect("/");
        }
    }
}