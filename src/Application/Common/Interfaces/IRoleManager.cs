using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IRoleManager
    {
        Task<Result> CreateRole(AppRole role);

        Task<bool> AnyRoles();
    }
}