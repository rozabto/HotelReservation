using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Admin.Commands.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employee;
        private readonly IUserManager _userManager;

        public CreateEmployeeHandler(IEmployeeRepository employee, IUserManager userManager)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.UserId)
                ?? throw new NotFoundException("User Id", request.UserId);

            await _employee.Create(new Employee
            {
                EGN = request.EGN,
                IsActive = true,
                MiddleName = request.MiddleName,
                UserId = user.Id
            }, cancellationToken);

            await _userManager.AddUserToRole(user, RoleType.Employee);

            return Unit.Value;
        }
    }
}
