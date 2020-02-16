using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<Result> CreateUser(AppUser user, string password, RoleType roleType = RoleType.User);
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> GetUserById(string userId);
        Task<Result> AddUserToRole(AppUser user, RoleType roleType);
    }
}
