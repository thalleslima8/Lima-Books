using AutoFixture;
using Bogus;
using LimaBooks.Domain.Core;
using LimaBooks.Service.BookService;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Enums;
using Serilog;

namespace LimaBooks.Service.SeedService
{
    public class SeedService : ISeedService
    {
		private readonly IBookService _bookService;
		private readonly Faker _faker = new Faker();

        public SeedService(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<object> Seed()
        {
			try
			{
                if (await _bookService.Any())
                    throw new Exception("Already Seeded.");

                var samples = new Faker<BookDto>("pt_BR")
                    .RuleFor(b => b.Availability, f => f.Random.Bool())
                    .RuleFor(b => b.Title, f => f.Company.CatchPhrase())
                    .RuleFor(b => b.CreatedAt, f => f.Date.Between(new DateTime(2022, 01, 01), new DateTime(2023, 09, 20)))
                    .RuleFor(b => b.UpdatedAt, f => f.Date.Between(new DateTime(2022, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.ISBN, f => f.Random.AlphaNumeric(10))
                    .RuleFor(b => b.Genre, f => f.Lorem.Word())
                    .RuleFor(b => b.PublishedDate, f=> f.Date.Between(new DateTime(2000, 01, 02), new DateTime(2023, 09, 19)))
                    .RuleFor(b => b.Publisher, f=> f.Company.CompanyName())
                    .RuleFor(b => b.Description, f => f.Lorem.Paragraph(1))
                    .RuleFor(b => b.PageCount, f => f.Random.Int(20, 700))
                    .RuleFor(b => b.Language, f => f.Address.Country())
                    .RuleFor(b => b.CoverImageURL, f => f.Image.LoremPixelUrl())
                    .RuleFor(b => b.ReadingStatus, f => f.PickRandom<ReadingStatus>())
                    .Generate(15)
                    ;

                var result = await _bookService.SaveBatch(samples);

                return new {
                    Seeded = true,
                    Message = $"{(samples?.Count ?? 0)} were added to DB."
                };
            }
			catch (Exception e)
			{
                Log.Logger.Error($"Error on seeding. {e.Message}", e);
                throw;
            }
        }
    }
}
