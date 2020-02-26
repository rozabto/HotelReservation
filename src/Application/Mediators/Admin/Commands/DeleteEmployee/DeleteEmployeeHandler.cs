using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Admin.Commands.DeleteEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employee;
        private readonly IUserManager _userManager;
        private readonly IMemoryService _memory;

        public DeleteEmployeeHandler(IEmployeeRepository employee, IUserManager userManager, IMemoryService memory)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employee.GetEmployeeWithUser(request.Id, cancellationToken)
                ?? throw new NotFoundException("Employee", request.Id);

            await _userManager.RemoveUserFromRole(employee.User, RoleType.Employee);

            await _employee.Delete(employee, cancellationToken);

            var role = employee.User.UsersRoles.FirstOrDefault(f => f.Role.Name == "Employee");
            if (role != null)
                employee.User.UsersRoles.Remove(role);

            _memory.SetValue(employee.User.UserName, employee.User);

            return Unit.Value;
        }
    }
}
