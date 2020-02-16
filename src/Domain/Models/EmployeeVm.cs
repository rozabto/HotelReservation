using System;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Domain.Models
{
    public class EmployeeVm : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public ulong EGN { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiredOnDate { get; set; }
        public string HiredByUsername { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Employee, EmployeeVm>()
                .ForMember(f => f.FullName, f => f.MapFrom(s => s.User.FirstName + ' ' + s.MiddleName + ' ' + s.User.LastName))
                .ForMember(f => f.HiredOnDate, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.HiredByUsername, f => f.MapFrom(s => s.CreatedByUser.UserName));
    }
}
