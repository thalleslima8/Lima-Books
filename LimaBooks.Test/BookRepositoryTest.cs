using LimaBooks.Domain.Core;
using LimaBooks.Repository.Interfaces;

namespace LimaBooks.Test
{
    public class BookRepositoryTest : BaseRepositoryTest<Book>, IBookRepository
    {
        public BookRepositoryTest(IEnumerable<Book>? values = null) : base(values)
        {
            
        }
    }
}
