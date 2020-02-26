using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Admin.Commands.EditEmployee
{
    public class EditEmployeeCommand : IRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        public ulong? EGN { get; set; }
    }
}
