using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Domain.Models
{
    public class EmployeeShortVm : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime HiredOnDate { get; set; }
        public IReadOnlyList<int> CreatedRoomsPerWeek { get; set; }

        public void Mapping(Profile profile)
        {
            DateTime afterDate = default;
            profile.CreateMap<Employee, EmployeeShortVm>()
                .ForMember(f => f.FullName, f => f.MapFrom(s => s.User.FirstName + ' ' + s.MiddleName + ' ' + s.User.LastName))
                .ForMember(f => f.HiredOnDate, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.Email, f => f.MapFrom(s => s.User.Email))
                .ForMember(f => f.CreatedRoomsPerWeek, f => f.MapFrom(s => s.User.CreatedRooms.Where(w => w.CreatedOn > afterDate).OrderByDescending(w => w.CreatedOn).Select(w => 1 + (w.CreatedOn.DayOfYear - 1) / 7).ToArray()));
        }
    }

    public class EmployeeVm : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ulong EGN { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiredOnDate { get; set; }
        public IReadOnlyList<int> CreatedRoomsPerWeek { get; set; }

        public void Mapping(Profile profile)
        {
            DateTime afterDate = default;
            profile.CreateMap<Employee, EmployeeVm>()
                .ForMember(f => f.FullName, f => f.MapFrom(s => s.User.FirstName + ' ' + s.MiddleName + ' ' + s.User.LastName))
                .ForMember(f => f.HiredOnDate, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.Email, f => f.MapFrom(s => s.User.Email))
                .ForMember(f => f.CreatedRoomsPerWeek, f => f.MapFrom(s => s.User.CreatedRooms.Where(w => w.CreatedOn > afterDate).OrderByDescending(w => w.CreatedOn).Select(w => 1 + (w.CreatedOn.DayOfYear - 1) / 7).ToArray()));
        }
    }
}