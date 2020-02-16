using System.Collections.Generic;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Employees.Commands.CreateHotelRoom
{
    public class CreateHotelRoomCommand : IRequest
    {
        public int Capacity { get; set; }
        public RoomType RoomType { get; set; }
        public decimal PriceForAdults { get; set; }
        public decimal PriceForChildren { get; set; }
        public int RoomNumber { get; set; }
        public IReadOnlyList<IFormFile> Images { get; set; }
    }
}
