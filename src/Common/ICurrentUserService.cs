using Domain.Entities;

namespace Common
{
    public interface ICurrentUserService
    {
        string Ip { get; set; }
        AppUser User { get; set; }
        bool IsAuthenticated { get; set; }
    }
}