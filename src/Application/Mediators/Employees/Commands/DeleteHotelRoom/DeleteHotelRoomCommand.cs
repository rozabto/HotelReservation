using MediatR;

namespace Application.Employees.Commands.DeleteHotelRoom
{
    public class DeleteHotelRoomCommand : IRequest
    {
        public string Id { get; set; }
    }
}
