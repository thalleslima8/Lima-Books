using AutoMapper;
using LimaBooks.Domain.Core;
using LimaBooks.Shared.Dtos;

namespace LimaBooks.Service.MapperService
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
        }
    }
}
