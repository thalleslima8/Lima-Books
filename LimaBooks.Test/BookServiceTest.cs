using AutoMapper;
using Bogus;
using LimaBooks.Repository.Interfaces;
using LimaBooks.Service.BookService;
using LimaBooks.Shared.Dtos;
using Xunit;

namespace LimaBooks.Test
{
    public class BookServiceTest
    {
        private readonly IMapper _mapper;

        public BookServiceTest(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IBookService PrepareService(IBookRepository bookRepositoryTest = null)
        {
            bookRepositoryTest ??= new BookRepositoryTest();


            return new BookService(bookRepositoryTest, _mapper);
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

        [Fact]
        public async Task Test_SaveNew_Book()
        {
            //Arranje

            var dto = CreateDto();

            var repository = new BookRepositoryTest();

            var service = PrepareService(repository);

            //Act
            
            var result = await service.Save(dto);

            //Assert
            
            Assert.NotNull(result);
            Assert.True(await repository.Any(x => x.Id > 0));
        }
    }
}