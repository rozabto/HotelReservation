using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Admin.Commands.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employee;
        private readonly IUserManager _userManager;
        private readonly IMemoryService _memory;

        public CreateEmployeeHandler(IEmployeeRepository employee, IUserManager userManager, IMemoryService memory)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
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

            _memory.SetValue(user.UserName, user);

            return Unit.Value;
        }
    }
}