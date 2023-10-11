using LimaBooks.Shared.Enums;

namespace LimaBooks.Shared.Dtos
{
    public class BookDto : DtoBase
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }
        public bool Availability { get; set; }
        public string CoverImageURL { get; set; }
        public ReadingStatus ReadingStatus { get; set; }
    }
}
