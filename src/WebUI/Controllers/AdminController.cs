using System.Threading.Tasks;
using Application.Admin.Commands.CreateEmployee;
using Application.Admin.Commands.DeleteEmployee;
using Application.Admin.Commands.EditEmployee;
using Application.Admin.Queries.GetEmployee;
using Application.Admin.Queries.ListOfEmployees;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public async Task<IActionResult> Index(int? page) =>
            View(await Mediator.Send(new ListOfEmployeesQuery { Page = page ?? 0 }));

        public IActionResult Create() => View();

        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Employee"] = (await Mediator.Send(new GetEmployeeQuery { Id = id })).Employee;
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteEmployeeCommand { Id = id });
            return Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await Mediator.Send(command);
            }
            catch (ModelStateException ex)
            {
                if (ex.ModelStates.Count > 0)
                {
                    foreach (var key in ex.ModelStates)
                        ModelState.AddModelError(key, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View();
            }
            return Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Employee"] = (await Mediator.Send(new GetEmployeeQuery { Id = command.Id })).Employee;
                return View();
            }

            await Mediator.Send(command);
            return Redirect(nameof(Index));
        }
    }
}