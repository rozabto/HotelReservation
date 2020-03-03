using System;
using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Common.Validators;
using MediatR;

namespace Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommand : IRequest<string>
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string RoomId { get; set; }

        [Required]
        [OverCurrrentDate]
        [FromBeforeTo]
        public DateTime From { get; set; }

        [Required]
        [OverCurrrentDate]
        [ToAfterFrom]
        public DateTime To { get; set; }

        [Required]
        public ReservationInclude Include { get; set; }

        [Range(0, 1000)]
        public int? Children { get; set; }

        [Range(0, 1000)]
        public int? Adults { get; set; }
    }
}