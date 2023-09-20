using BudgEase.Repository.Base;
using LimaBooks.Domain.Base;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;
using Serilog;

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

        public abstract Task<D> ToDto(T entity);
        public abstract Task<T> ToModel(D dto);

        public virtual async Task Delete(int? id)
        {
            if (!id.HasValue)
                return;

            await _repository.DeleteById(id.Value);
        }

        public virtual async Task<IEnumerable<D>> Get(F filter)
        {
            var query = await GetQuery();

            if (filter?.Id != null)
                query = query.Where(x => x.Id == filter.Id);

            query = query.Where(x => x.IsActive);

            query = query.Skip(filter.Page.Value * filter.Size.Value).Take(filter.Size.Value);

            var models = query.ToList();

            var result = new List<D>();

            foreach (var model in models)
            {
                D? itemDto = model is null ? default(D) : await ToDto(model);

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

                var model = await ToModel(dto);

                if (model.Id.HasValue)
                    return await Update(dto);

                var result = await _repository.SaveAsync(model);

                return await ToDto(result);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error on save. {e.Message}", e);
                throw;
            }
        }

        public virtual async Task<D> Update(D dto)
        {
            var model = await ToModel(dto);

            if (!model.Id.HasValue)
                return await ToDto(await _repository.SaveAsync(model));

            await _repository.SaveChanges();

            return await ToDto(model);
        }
    }
}
