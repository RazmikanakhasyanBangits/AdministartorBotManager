using Microsoft.EntityFrameworkCore.Query;
using Repository.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<T> GetDetailsAsync(object Id);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetDetailsAsync(Expression<Func<T, bool>> predicate);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, bool? disableTracking = null);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, bool? disableTracking = null);
        Task<T> GetDetailsAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<PagedResult<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, bool? disableTracking = null, int page = 1, int pageSize = 30);
    }
}
