using BudgEase.Repository.Base;
using LimaBooks.Domain.Base;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;
using Serilog;
using System.Linq;

namespace LimaBooks.Service.Base
{
    public abstract class BaseService<T, D, F, TRepository> : IBaseService<T, D, F>
        where T : ModelBase, new()
        where D : DtoBase, new()
        where F : FilterBase, new()
        where TRepository : IBaseRepository<T>
    {
        protected readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {                
            _repository = repository;
        }

        public abstract D ToDto(T entity);
        public abstract T ToModel(D dto);

        public virtual async Task Delete(int? id)
        {
            if (!id.HasValue)
                return;

            await _repository.DeleteById(id.Value);
        }

        public virtual async Task<IEnumerable<D>> Get(IQueryable<T> query, F filter)
        {
            if (filter.Id != null)
                query = query.Where(x => x.Id == filter.Id);

            if (filter.CreatedAt != null)
                query = query.Where(x => x.CreatedAt.Date == filter.CreatedAt.Value.Date);

            if (filter.UpdatedAt != null)
                query = query.Where(x => x.UpdatedAt.Date == filter.UpdatedAt.Value.Date);

            query = query.Where(x => filter.IsActive == x.IsActive);

            query = query.Skip(filter.Page.Value * filter.Size.Value).Take(filter.Size.Value);

            var models = query.ToList();

            var result = new List<D>();

            foreach (var model in models)
            {
                D? itemDto = model is null ? default(D) : ToDto(model);

                if (itemDto is not null)
                    result.Add(itemDto);
            }

            return result;
        }

        public virtual async Task<IQueryable<T>> GetQuery()
        {
            return await _repository.GetAll();
        }

        public virtual async Task<D> Save(D dto)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                var model = ToModel(dto);

                if (model.Id.HasValue)
                    return await Update(dto);

                var result = await _repository.SaveAsync(model);

                return ToDto(result);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on save. {e.Message}", e);
                throw;
            }
        }

        public virtual async Task<D> Update(D dto)
        {
            var model = ToModel(dto);

            if (!model.Id.HasValue)
                return ToDto(await _repository.SaveAsync(model));

            await _repository.SaveChanges();

            return ToDto(model);
        }

        public async Task<IEnumerable<D>> SaveBatch(IEnumerable<D> dtos)
        {
            try
            {
                var models = dtos.Select(dto => ToModel(dto)).ToList();

                var savedModels = await _repository.SaveBatchAsync(models);

                dtos = savedModels.Select(model => ToDto(model)).ToList();

                return dtos;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on save batch. {e.Message}", e);
                throw;
            }
        }

        public abstract Task<IEnumerable<D>> GetByFilter(F filter);
    }
}
