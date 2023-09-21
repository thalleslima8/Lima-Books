using AutoMapper;
using LimaBooks.Domain.Core;
using LimaBooks.Repository.Interfaces;
using LimaBooks.Repository.Repositories;
using LimaBooks.Service.Base;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;

namespace LimaBooks.Service.BookService
{
    public class BookService : BaseService<Book, BookDto, BookFilter, BookRepository>, IBookService
    {
        private readonly IMapper _mapper;
        public BookService(IBookRepository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<bool> Any()
        {
            var any = await _repository.FirstOrDefaultAsync(x => x.Id > 0);

            return any != null;
        }

        public override BookDto ToDto(Book entity)
        {
            var dto = _mapper.Map<BookDto>(entity);
            return dto;
        }

        public override Book ToModel(BookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            return book;
        }
    }
}
