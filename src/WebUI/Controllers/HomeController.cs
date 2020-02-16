using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Home.Queries.Checkout;
using Application.Home.Queries.GetHomePage;
using Application.Home.Queries.GetHotelRoom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IActionResult> Index(int? page) =>
            View(await Mediator.Send(new GetHomePageQuery { Page = page ?? 0 }));

        public async Task<IActionResult> Details(string id) =>
            View(await Mediator.Send(new GetHotelRoomQuery { Id = id }));

        public async Task<IActionResult> Checkout() =>
            View(await Mediator.Send(new CheckoutQuery()));

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
