using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.System.Commands.SeedData
{
    public class SeedDataHandler : IRequestHandler<SeedDataCommand>
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public SeedDataHandler(IUserManager userManager, IRoleManager roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<Unit> Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            if (await _roleManager.AnyRoles())
                return Unit.Value;

            var adminRole = new AppRole
            {
                Name = "Admin"
            };

            await _roleManager.CreateRole(adminRole);

            await _roleManager.CreateRole(
                new AppRole
                {
                    Name = "Employee"
                });

            var admin = new AppUser
            {
                Email = "adminUser@hotelReservation.com",
                EmailConfirmed = true,
                FirstName = "",
                LastName = "",
                UserName = "HotelReservationManager"
            };
            await _userManager.CreateUser(admin, "CreativePassword123", RoleType.Admin);

            return Unit.Value;
        }
    }
}
