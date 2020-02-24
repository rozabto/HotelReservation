using System.Threading.Tasks;
using Application.Search.Queries.SearchUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Api
{
    [Authorize(Roles = "Employee,Admin")]
    public class UserController : BaseApiController
    {
        [HttpGet("{term}")]
        public async Task<IActionResult> GetUsers(string term) =>
            Ok(await Mediator.Send(new SearchUsersQuery { Term = term }));
    }
}