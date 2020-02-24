using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public Employee()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string MiddleName { get; set; }
        public ulong EGN { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}