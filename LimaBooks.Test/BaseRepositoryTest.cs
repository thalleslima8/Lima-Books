using Bogus;
using BudgEase.Repository.Base;
using LimaBooks.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace LimaBooks.Test
{
    public class BaseRepositoryTest<T> : IBaseRepository<T> where T : ModelBase
    {
        protected IQueryable<T> _data;
        private readonly Faker _faker = new Faker();

        public BaseRepositoryTest(IEnumerable<T>? values)
        {
            _data = values?.AsQueryable() ?? Enumerable.Empty<T>().AsQueryable();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return _data.Where(predicate).Any();
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
                return await _data.Where(predicate).ToListAsync();
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
                return (await FindAsync(predicate))?.FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on FirstOrDefaultAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return _data.AsQueryable();
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return await _data.FirstOrDefaultAsync(x => x.Id == id);
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
                var list = await _data.ToListAsync();
                var now = DateTime.UtcNow;
                entity.UpdatedAt = now;
                entity.CreatedAt = now;
                entity.Id = _faker.Random.Int(1,10000);
                list.Add(entity);
                _data = list.AsQueryable();
                return entity;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on SaveAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<T>> SaveBatchAsync(IEnumerable<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (entity == null)
                        continue;

                    await SaveAsync(entity);
                }

                return entities;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on SaveAsync. {e.Message}", e);
                throw;
            }
        }

        public async Task SaveChanges() { }

        public async Task<bool> Update(T entity)
        {
            try
            {
                var list = _data.Where(x => x.Id != entity.Id).ToList();
                entity.UpdatedAt = DateTime.UtcNow;
                list.Add(entity);
                _data = list.AsQueryable();

                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on Update. {e.Message}", e);
                throw;
            }
        }
    }
}
