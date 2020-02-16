using System;
using Domain.Entities;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public string CreatedByUserId { get; set; }
        public AppUser CreatedByUser { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
