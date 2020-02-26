using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;
using Domain.Models;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<Result> CreateUser(AppUser user, string password, RoleType roleType = RoleType.User);

        Task<AppUser> GetUserByUsername(string username);

        Task<AppUser> GetUserById(string userId);

        Task<AppUser> GetUserByEmail(string email);

        Task<Result> AddUserToRole(AppUser user, RoleType roleType);

        Task<Result> RemoveUserFromRole(AppUser user, RoleType roleType);

        Task<List<UserVm>> SearchUsers(string term);
    }
}