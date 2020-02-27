using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Employees.Commands.CreateHotelRoom
{
    public class CreateHotelRoomHandler : IRequestHandler<CreateHotelRoomCommand>
    {
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly IHotelRoomImageRepository _hotelRoomImage;
        private readonly IEmployeeRepository _employee;
        private readonly ICurrentUserService _currentUser;
        private readonly IImageService _image;

        public CreateHotelRoomHandler(IHotelRoomRepository hotelRoom, IHotelRoomImageRepository hotelRoomImage, IEmployeeRepository employee, ICurrentUserService currentUser, IImageService image)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _hotelRoomImage = hotelRoomImage ?? throw new ArgumentNullException(nameof(hotelRoomImage));
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
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

            var emplyee = await _employee.GetEmployeeByUser(_currentUser.User.Id, cancellationToken);
            if (emplyee?.IsActive == false)
            {
                emplyee.IsActive = true;
                await _employee.Update(emplyee, cancellationToken);
            }

            return Unit.Value;
        }
    }
}