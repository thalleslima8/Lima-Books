using LimaBooks.Domain.Core;
using LimaBooks.Service.Base;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;

namespace LimaBooks.Service.BookService
{
    public interface IBookService : IBaseService<Book, BookDto, BookFilter>
    {
        Task<bool> Any();
    }
}
