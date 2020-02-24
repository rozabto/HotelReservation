using System;

namespace Domain.Entities
{
    public class HotelRoomImage
    {
        public HotelRoomImage()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string Image { get; set; }
        public string HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }
    }
}