using System;
using Domain.Entities;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedById { get; set; }
        public AppUser EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
    }
}