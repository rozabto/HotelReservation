using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDateTime _date;

        public UserManagerService(UserManager<AppUser> userManager, IDateTime date)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<Result> CreateUser(AppUser user, string password, RoleType roleType = RoleType.User)
        {
            user.CreatedOn = _date.Now;

            var result = await _userManager.CreateAsync(user, password);

            if (roleType != RoleType.User)
            {
                await _userManager.AddToRoleAsync(user,
                    roleType switch
                    {
                        RoleType.Admin => "Admin",
                        RoleType.Employee => "Employee",
                        _ => string.Empty
                    }
                );
            }

            return result.ToApplicationResult();
        }

        public Task<AppUser> GetUserByUsername(string username) =>
            _userManager.Users
                .Include(f => f.UsersRoles)
                .ThenInclude(f => f.Role)
                .FirstOrDefaultAsync(f => f.NormalizedUserName == _userManager.NormalizeName(username));

        public Task<AppUser> GetUserById(string userId) =>
            _userManager.Users
                .Include(f => f.UsersRoles)
                .ThenInclude(f => f.Role)
                .FirstOrDefaultAsync(f => f.Id == userId);

        public async Task<Result> AddUserToRole(AppUser user, RoleType roleType) =>
            (await _userManager.AddToRoleAsync(user,
                roleType switch
                {
                    RoleType.Admin => "Admin",
                    RoleType.Employee => "Employee",
                    _ => throw new NotImplementedException("Role Type of User is not implemented")
                }
            )).ToApplicationResult();
    }
}
