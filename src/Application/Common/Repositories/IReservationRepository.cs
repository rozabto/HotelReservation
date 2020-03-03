using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;

namespace Application.Common.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<bool> CanReserve(string roomId, DateTime to, DateTime from, CancellationToken token);

        Task<Reservation> FindByRoomId(string roomId, string userId, CancellationToken token);

        Task<List<ReservationVm>> GetUserReservations(string userId, int page, int pageCount, CancellationToken token);

        Task DeleteExpired(DateTime date, CancellationToken token);
    }
}