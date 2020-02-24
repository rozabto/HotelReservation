using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Employees.Commands.CreateHotelRoom
{
    public class CreateHotelRoomHandler : IRequestHandler<CreateHotelRoomCommand>
    {
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly IHotelRoomImageRepository _hotelRoomImage;
        private readonly IImageService _image;

        public CreateHotelRoomHandler(IHotelRoomRepository hotelRoom, IHotelRoomImageRepository hotelRoomImage, IImageService image)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _hotelRoomImage = hotelRoomImage ?? throw new ArgumentNullException(nameof(hotelRoomImage));
            _image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public async Task<Unit> Handle(CreateHotelRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new HotelRoom
            {
                Capacity = request.Capacity,
                IsEmpty = true,
                RoomNumber = request.RoomNumber,
                RoomType = request.RoomType,
                PriceForAdults = request.PriceForAdults,
                PriceForChildren = request.PriceForChildren,
                FoodPrice = request.FoodPrice,
                Name = request.Name,
                RoomPrice = request.RoomPrice
            };

            await _hotelRoom.Create(room, cancellationToken);

            var images = new List<HotelRoomImage>(request.Images.Count);

            foreach (var image in await _image.SaveImages(request.Images))
            {
                images.Add(new HotelRoomImage
                {
                    HotelRoomId = room.Id,
                    Image = image
                });
            }

            await _hotelRoomImage.CreateMany(images, cancellationToken);

            return Unit.Value;
        }
    }
}