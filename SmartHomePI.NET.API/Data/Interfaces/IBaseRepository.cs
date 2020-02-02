using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHomePI.NET.API.Data
{
    public interface IBaseRepository<T>
    {
        Task Insert(T entity);

        Task Update(T item);

        Task Delete(int id);

        Task Delete(T entity);

        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}