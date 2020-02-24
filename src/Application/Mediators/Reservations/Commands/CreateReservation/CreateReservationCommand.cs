using System;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validators;
using MediatR;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommand : IRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string RoomId { get; set; }

        [Required]
        [OverCurrrentDate]
        public DateTime From { get; set; }

        [Required]
        [OverCurrrentDate]
        public DateTime To { get; set; }

        [Required]
        public bool IncludeFood { get; set; }

        [Required]
        public bool AllInclusive { get; set; }

        [Range(0, 20)]
        public int? Children { get; set; }

        [Range(0, 20)]
        public int? Adults { get; set; }
    }
}