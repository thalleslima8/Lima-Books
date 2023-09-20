using BudgEase.Repository.Base;
using LimaBooks.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace LimaBooks.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : ModelBase
    {
        private readonly MySqlContext _context;
        private readonly DbSet<T> DbSet;
        private bool _disposed;

        public BaseRepository(MySqlContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public async Task SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on Save Changes. {e.Message}", e);
                throw;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var item = await GetById(id);

                if (item == null)
                    return false;

                item.IsActive = false;
                item.UpdatedAt = DateTime.UtcNow;

                await SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on Delete. {e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await DbSet.Where(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on FindAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return (await FindAsync(predicate)).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on FirstOrDefaultAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<IQueryable<T>> GetAll()
        {
            try
            {
                return DbSet.AsQueryable();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on GetAll. {e.Message}", e);
                throw;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on GetById. {e.Message}", e);
                throw;
            }
        }

        public async Task<T> SaveAsync(T entity)
        {
            try
            {
                var now = DateTime.UtcNow;
                entity.UpdatedAt = now;
                entity.CreatedAt = now;
                var result = await DbSet.AddAsync(entity);
                await SaveChanges();
                return result.Entity as T;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on SaveAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate) => await DbSet.AnyAsync(predicate);

        public async Task<IEnumerable<T>> SaveBatchAsync(IEnumerable<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (entity == null)
                        continue;

                    var now = DateTime.UtcNow;
                    entity.UpdatedAt = now;
                    entity.CreatedAt = now;

                    await DbSet.AddAsync(entity);
                }

                return entities;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on SaveAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.UtcNow;
                DbSet.Update(entity);

                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on Update. {e.Message}", e);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                (_context as IDisposable).Dispose();
            _disposed = true;
        }
    }
}
