using LimaBooks.Domain.Core;
using LimaBooks.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LimaBooks.Repository
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }

        DbSet<Book> _books;

        protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        {
            base.OnConfiguring(optBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Book>(c =>
            {
                c.ToTable("Books");
                c.HasKey(x => x.Id);
                c.Property(x => x.Id).ValueGeneratedOnAdd();
                c.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();

                c.HasIndex(x => x.Title).IsUnique();

                c.Property(x => x.Title).IsRequired();
                c.Property(x => x.Author).IsRequired();

                c.Property(x => x.ReadingStatus).HasConversion(new EnumToStringConverter<ReadingStatus>());
            });

            base.OnModelCreating(mb);
        }
    }
}
