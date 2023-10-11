using LimaBooks.Domain.Core;
using LimaBooks.Repository.Interfaces;
using LimaBooks.Tests;

namespace LimaBooks.Test
{
    public class BookRepositoryTest : BaseRepositoryTest<Book>, IBookRepository
    {
        public BookRepositoryTest(params Book[] values) : base(values)
        {
            
        }
    }
}
