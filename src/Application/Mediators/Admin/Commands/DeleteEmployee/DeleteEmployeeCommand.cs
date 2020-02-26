using MediatR;

namespace Application.Admin.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest
    {
        public string Id { get; set; }
    }
}
