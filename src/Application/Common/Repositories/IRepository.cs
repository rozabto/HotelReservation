using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Repositories
{
    public interface IRepository<T>
    {
        ValueTask<T> GetById(object id, CancellationToken token);

        Task<Result> Create(T entity, CancellationToken token);

        Task<Result> CreateMany(IEnumerable<T> entities, CancellationToken token);

        Task<Result> Delete(object id, CancellationToken token);

        Task<Result> Delete(T entity, CancellationToken token);

        Task<Result> DeleteMany(IEnumerable<T> entities, CancellationToken token);

        Task<Result> Update(T entity, CancellationToken token);

        Task<Result> UpdateMany(IEnumerable<T> entities, CancellationToken token);
    }
}