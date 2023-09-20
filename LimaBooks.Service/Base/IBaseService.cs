using LimaBooks.Domain.Base;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;

namespace LimaBooks.Service.Base
{
    public interface IBaseService<T, D, F>
        where T : ModelBase
        where D : DtoBase
        where F : FilterBase
    {
        Task<D> Save(D dto);
        Task Delete(int? id);
        Task<D> Update(D dto);
        Task<IEnumerable<D>> Get(F filter);
        Task<IQueryable<T>> GetQuery();
    }
}
