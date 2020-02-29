using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Home.Queries.GetHotelRoom;
using Application.Search.Queries.SearchHotelRooms;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebUI.Common.Models;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Details(string id) =>
            View(await Mediator.Send(new GetHotelRoomQuery { Id = id }));

        public IActionResult Privacy() => View();

        public IActionResult Contacts() => View();

        [HttpPost]
        public IActionResult Contacts(object model) => Redirect("/");

        public async Task<IActionResult> Search(string term, DateTime? from, DateTime? to, int? capacity, int? page, RoomType? type, decimal? start, decimal? end, SortBy sort = default) =>
            View(await Mediator.Send(new SearchHotelRoomsQuery
            {
                Term = term,
                AvailableFrom = from,
                AvailableTo = to,
                Capacity = capacity,
                Page = page ?? 0,
                RoomType = type,
                Start = start,
                End = end,
                SortBy = sort
            }));

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}