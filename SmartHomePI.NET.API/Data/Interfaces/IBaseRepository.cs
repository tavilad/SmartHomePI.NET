using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHomePI.NET.API.Data
{
    public interface IBaseRepository<T>
    {
        void Insert(T entity);

        void Update(T item);

        void Delete(int id);

        void Delete(T entity);

        Task<T> GetByID(int id);

        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}