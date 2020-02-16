using MediatR;

namespace Application.Admin.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest
    {
        public string MiddleName { get; set; }
        public ulong EGN { get; set; }
        public string UserId { get; set; }
    }
}
