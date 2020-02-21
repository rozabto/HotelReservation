using MediatR;

namespace Application.Reservations.Queries.Reservations
{
    public class ReservationQuery : IRequest<ReservationsResponse>
    {
        public int Page { get; set; }
    }
}
