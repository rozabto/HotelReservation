using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Domain.Models
{
    public class UserVm : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, UserVm>()
                .ForMember(f => f.FullName, f => f.MapFrom(s => s.FirstName + ' ' + s.LastName));
    }
}