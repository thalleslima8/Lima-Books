using AutoMapper;
using Bogus;
using LimaBooks.Domain.Core;
using LimaBooks.Repository.Interfaces;
using LimaBooks.Service.BookService;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;
using Moq;

namespace LimaBooks.Test
{
    public class BookServiceTest
    {
        public BookServiceTest()
        {
        }

        public IBookService PrepareService(IBookRepository? bookRepositoryTest = null, BookDto? dto = null, Book? model = null)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BookDto, Book>();
                cfg.CreateMap<Book, BookDto>();
            });

            bookRepositoryTest ??= new BookRepositoryTest();
            var mappingService = config.CreateMapper();

            return new BookService(bookRepositoryTest, mappingService);
        }

        public BookDto CreateDto()
        {
            return new Faker<BookDto>("pt_BR")
                    .RuleFor(b => b.Availability, f => f.Random.Bool())
                    .RuleFor(b => b.Title, f => f.Company.CatchPhrase())
                    .RuleFor(b => b.CreatedAt, f => f.Date.Between(new DateTime(2022, 01, 01), new DateTime(2023, 09, 20)))
                    .RuleFor(b => b.UpdatedAt, f => f.Date.Between(new DateTime(2022, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.ISBN, f => f.Random.AlphaNumeric(10))
                    .RuleFor(b => b.Genre, f => f.Lorem.Word())
                    .RuleFor(b => b.PublishedDate, f => f.Date.Between(new DateTime(2000, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.Description, f => f.Lorem.Paragraph(1))
                    .RuleFor(b => b.PageCount, f => f.Random.Int(20, 700))
                    .RuleFor(b => b.Language, f => f.Address.Country())
                    .RuleFor(b => b.CoverImageURL, f => f.Image.LoremPixelUrl());
        }

        public Faker<Book> CreateBook(int? id = null,
                                      string author = null,
                                      string description = null,
                                      string genre = null,
                                      string isbn = null,
                                      string language = null)
        {
            return new Faker<Book>("pt_BR")
                    .RuleFor(b => b.IsActive, f => true)
                    .RuleFor(b => b.Id, f => id.HasValue ? id : f.Random.Int(1, 70))
                    .RuleFor(b => b.Availability, f => f.Random.Bool())
                    .RuleFor(b => b.Title, f => f.Company.CatchPhrase())
                    .RuleFor(b => b.CreatedAt, f => f.Date.Between(new DateTime(2022, 01, 01), new DateTime(2023, 09, 20)))
                    .RuleFor(b => b.UpdatedAt, f => f.Date.Between(new DateTime(2022, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Author, f => string.IsNullOrEmpty(author) ? f.Name.FullName() : author)
                    .RuleFor(b => b.ISBN, f => string.IsNullOrEmpty(isbn) ? f.Random.AlphaNumeric(10) : isbn)
                    .RuleFor(b => b.Genre, f => string.IsNullOrEmpty(genre) ? f.Lorem.Word() : genre)
                    .RuleFor(b => b.PublishedDate, f => f.Date.Between(new DateTime(2000, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.Description, f => string.IsNullOrEmpty(description) ? f.Lorem.Paragraph(1) : description)
                    .RuleFor(b => b.PageCount, f => f.Random.Int(20, 700))
                    .RuleFor(b => b.Language, f => string.IsNullOrEmpty(language) ? f.Address.Country() : language)
                    .RuleFor(b => b.CoverImageURL, f => f.Image.LoremPixelUrl());
        }

        public IEnumerable<Book> CreateBookList(Faker<Book> fakerBook, int qtd = 1)
        {
            return fakerBook.Generate(qtd);
        }

        [Fact]
        public async Task Test_SaveNew_Book()
        {
            //Arranje

            var dto = CreateDto();

            var repository = new BookRepositoryTest();

            var service = PrepareService(repository, dto);

            //Act

            var result = await service.Save(dto);

            //Assert

            Assert.NotNull(result);
            Assert.True(await repository.Any(x => x.Id > 0));
        }

        [Fact]        
        public async Task Test_Get_Books()
        {
            //Arranje

            var author = "Teste Author";
            var book = CreateBook(author: author);
            var bookList = CreateBookList(book, 5);

            var repository = new BookRepositoryTest(bookList.ToArray());

            var service = PrepareService(repository, null, book);

            var filter = GetFilter(author: author);

            //Act

            var result = await service.GetByFilter(filter);

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var item = result.FirstOrDefault();

            Assert.NotNull(item);

            Assert.Contains(bookList, x => x.Title == item.Title);
            Assert.Contains(bookList, x => x.Author == item.Author);
        }

        private static BookFilter GetFilter(int? id = null, 
                                         string author = null,
                                         string description = null,
                                         string genre = null,
                                         string isbn = null,
                                         string language = null)
        {
            return new BookFilter
            {
                Author = author,
                Description = description,
                Genre = genre,
                Id = id,
                ISBN = isbn,
                Language = language,
            };
        }
    }
}