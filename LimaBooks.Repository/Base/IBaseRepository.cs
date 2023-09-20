using LimaBooks.Domain.Base;
using System.Linq.Expressions;

namespace BudgEase.Repository.Base
{
    public interface IBaseRepository<T> where T : ModelBase
    {
        Task SaveChanges();
        Task<T> GetById(int id);
        Task<bool> DeleteById(int id);
        Task<IQueryable<T>> GetAll();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> Update(T entity);
        Task<T> SaveAsync(T entity);
        Task<IEnumerable<T>> SaveBatchAsync(IEnumerable<T> entities);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
    }
}
