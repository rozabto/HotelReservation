using System.Threading.Tasks;
using Application.Admin.Commands.CreateEmployee;
using Application.Admin.Queries.ListOfEmployees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index(int? page) =>
            View(await Mediator.Send(new ListOfEmployeesQuery { Page = page ?? 0 }));

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
                return View();

            await Mediator.Send(command);
            return Redirect(nameof(Index));
        }
    }
}
