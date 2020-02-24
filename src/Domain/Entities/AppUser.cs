using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
        {
            Reservations = new HashSet<Reservation>();
            UsersRoles = new HashSet<AppUserRole>();
            Id = Guid.NewGuid().ToString("N");
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }
        public DateTime CreatedOn { get; set; }
        public Employee Employee { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<AppUserRole> UsersRoles { get; set; }
    }
}