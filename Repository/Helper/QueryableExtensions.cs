using Microsoft.EntityFrameworkCore;

namespace Repository.Helper
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        where T : class
        {
            PagedResult<T> result = new()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
