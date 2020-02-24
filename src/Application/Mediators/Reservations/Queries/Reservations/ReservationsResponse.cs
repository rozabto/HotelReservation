using System.Collections.Generic;
using Domain.Models;

namespace Application.Reservations.Queries.Reservations
{
    public class ReservationsResponse
    {
        public IReadOnlyList<ReservationVm> Reservations { get; set; }
    }
}