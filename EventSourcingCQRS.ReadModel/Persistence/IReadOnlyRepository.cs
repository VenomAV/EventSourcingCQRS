using EventSourcingCQRS.ReadModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSourcingCQRS.ReadModel.Persistence
{
    public interface IReadOnlyRepository<T>
        where T : IReadEntity
    {
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdAsync(string id);
    }
}
