using MediatR;

namespace Application.Reservations.Commands.CompleteReservation
{
    public class CompleteReservationCommand : IRequest
    {
        public string TotalAmount { get; set; }
        public string Currency { get; set; }
        public string ResponseTimeStamp { get; set; }
        public string PPP_TransactionID { get; set; }
        public string Status { get; set; }
        public string ProductId { get; set; }
        public string AdvanceResponseChecksum { get; set; }
        public string Email { get; set; }
    }
}
