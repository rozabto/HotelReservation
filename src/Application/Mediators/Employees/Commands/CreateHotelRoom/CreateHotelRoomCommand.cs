using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validators;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Employees.Commands.CreateHotelRoom
{
    public class CreateHotelRoomCommand : IRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Capacity { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        [PriceRequired]
        [BothPricesRequired]
        [Range(0, int.MaxValue)]
        public decimal? PriceForAdults { get; set; }

        [BothPricesRequired]
        [Range(0, int.MaxValue)]
        public decimal? PriceForChildren { get; set; }

        [PriceRequired]
        [Range(0, int.MaxValue)]
        public decimal? RoomPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal FoodPrice { get; set; }

        [Required]
        [Range(0, 10000)]
        public int RoomNumber { get; set; }

        [Required]
        [Images]
        public IReadOnlyList<IFormFile> Images { get; set; }
    }
}