using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
        {
            UsersReservations = new HashSet<UserReservation>();
            UsersRoles = new HashSet<AppUserRole>();
            Id = Guid.NewGuid().ToString("N");
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }
        public DateTime CreatedOn { get; set; }
        public Employee Employee { get; set; }

        public ICollection<UserReservation> UsersReservations { get; set; }
        public ICollection<AppUserRole> UsersRoles { get; set; }
    }
}
