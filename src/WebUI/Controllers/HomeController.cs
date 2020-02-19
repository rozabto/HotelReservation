using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Home.Queries.GetHomePage;
using Application.Home.Queries.GetHotelRoom;
using Application.Search.Queries.SearchHotelRooms;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index(int? page) =>
            View(await Mediator.Send(new GetHomePageQuery { Page = page ?? 0 }));

        public async Task<IActionResult> Details(string id) =>
            View(await Mediator.Send(new GetHotelRoomQuery { Id = id }));

        public IActionResult Privacy() => View();

        public IActionResult Contacts() => View();

        public async Task<IActionResult> Search(string term, DateTime from, DateTime to, int capacity, int page) =>
            View(await Mediator.Send(new SearchHotelRoomsQuery
            {
                Term = term,
                AvailableFrom = from,
                AvailableTo = to,
                Capacity = capacity,
                Page = page
            }));

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
