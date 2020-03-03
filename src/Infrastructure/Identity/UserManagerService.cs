using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IDateTime _date;

        public UserManagerService(UserManager<AppUser> userManager, IMapper mapper, IDateTime date)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        public Task<AppUser> GetUserByEmail(string email) =>
            _userManager.Users
                .Include(f => f.UsersRoles)
                .ThenInclude(f => f.Role)
                .FirstOrDefaultAsync(f => f.NormalizedEmail == _userManager.NormalizeEmail(email));

        public async Task<Result> AddUserToRole(AppUser user, RoleType roleType) =>
            (await _userManager.AddToRoleAsync(user,
                roleType == RoleType.Admin || roleType == RoleType.Employee
                    ? roleType.ToString()
                    : throw new NotImplementedException("Role Type of User is not implemented")
            )).ToApplicationResult();

        public Task<List<UserVm>> SearchUsers(string term)
        {
            var name = '%' + _userManager.NormalizeName(term) + '%';
            var email = '%' + _userManager.NormalizeEmail(term) + '%';
            term = '%' + term + '%';
            return _userManager.Users
                .Include(f => f.UsersRoles)
                .ThenInclude(f => f.Role)
                .Where(f => !f.UsersRoles.Any(s => s.Role.Name == "Admin" || s.Role.Name == "Employee")
                    && (EF.Functions.Like(f.FirstName, term)
                    || EF.Functions.Like(f.LastName, term)
                    || EF.Functions.Like(f.NormalizedEmail, email)
                    || EF.Functions.Like(f.NormalizedUserName, name)))
                .ProjectTo<UserVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Result> RemoveUserFromRole(AppUser user, RoleType roleType)
        {
            return (await _userManager.RemoveFromRoleAsync(user,
                roleType == RoleType.Admin || roleType == RoleType.Employee
                    ? roleType.ToString()
                    : throw new NotImplementedException("Role Type of User is not implemented")
            )).ToApplicationResult();
        }
    }
}