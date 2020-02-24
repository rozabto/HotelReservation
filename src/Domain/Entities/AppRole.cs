using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole()
        {
            UsersRoles = new HashSet<AppUserRole>();
            Id = Guid.NewGuid().ToString("N");
        }

        public ICollection<AppUserRole> UsersRoles { get; set; }
    }
}