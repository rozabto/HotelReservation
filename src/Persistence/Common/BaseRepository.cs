using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IHotelReservationContext Context;

        protected IQueryable<T> Query =>
            Context.Set<T>()
            .AsNoTracking();

        protected BaseRepository(IHotelReservationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ValueTask<T> GetById(object id, CancellationToken token) =>
            Context.Set<T>()
                .FindAsync(new[] { id }, token);

        public Task<Result> Create(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                await Context.Set<T>().AddAsync(entity, token);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> CreateMany(IEnumerable<T> entities, CancellationToken token) =>
            HandleAction(async () =>
            {
                await Context.Set<T>().AddRangeAsync(entities, token);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(object id, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().Remove(await GetById(id, token));
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> DeleteMany(IEnumerable<T> entities, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().RemoveRange(entities);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Update(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().Update(entity);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> UpdateMany(IEnumerable<T> entities, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().UpdateRange(entities);
                await Context.SaveChangesAsync(token);
            });

        protected async Task<Result> HandleAction(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { ex.Message });
            }
            return Result.Success();
        }
    }
}