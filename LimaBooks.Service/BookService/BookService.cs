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

        public override async Task<IEnumerable<BookDto>> GetByFilter(BookFilter filter)
        {
            var query = await GetQuery();

            if (filter == null)
                throw new Exception("Oops! Filter can not be null!");

            if(!string.IsNullOrEmpty(filter.Author))
                query = query.Where(x => x.Author.Contains(filter.Author));

            if(!string.IsNullOrEmpty(filter.Description))
                query = query.Where(x => x.Description.Contains(filter.Description));

            if(!string.IsNullOrEmpty(filter.Title))
                query = query.Where(x => x.Title.Contains(filter.Title));

            if(!string.IsNullOrEmpty(filter.Publisher))
                query = query.Where(x => x.Publisher.Contains(filter.Publisher));

            if (!string.IsNullOrEmpty(filter.ISBN))
                query = query.Where(x => x.ISBN.Contains(filter.ISBN));

            if (!string.IsNullOrEmpty(filter.Genre))
                query = query.Where(x => x.Genre.Contains(filter.Genre));

            if (!string.IsNullOrEmpty(filter.Language))
                query = query.Where(x => x.Language.Contains(filter.Language));

            if (filter.PublishedDate != null)
                query = query.Where(x => x.PublishedDate.Date == filter.PublishedDate.Value.Date);

            return await Get(query, filter);
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
