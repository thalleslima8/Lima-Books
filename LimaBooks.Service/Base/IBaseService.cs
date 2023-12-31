﻿using LimaBooks.Domain.Base;
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
        Task<IEnumerable<D>> SaveBatch(IEnumerable<D> dtos);
        Task Delete(int? id);
        Task<D> Update(D dto);
        Task<IEnumerable<D>> Get(IQueryable<T> query, F filter);
        Task<IQueryable<T>> GetQuery();
        Task<IEnumerable<D>> GetByFilter(F filter);
    }
}
