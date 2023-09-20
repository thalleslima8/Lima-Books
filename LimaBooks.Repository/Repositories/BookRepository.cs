using LimaBooks.Domain.Core;
using LimaBooks.Repository.Base;
using LimaBooks.Repository.Interfaces;

namespace LimaBooks.Repository.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(MySqlContext context) : base(context)
        {                
        }
    }
}
