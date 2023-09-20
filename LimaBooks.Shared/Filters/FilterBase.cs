namespace LimaBooks.Shared.Filters
{
    public class FilterBase
    {
        public virtual int? Id { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual DateTime? UpdateAt { get; set; }
        public virtual bool IsActive { get; set; } = true;
        public virtual int? Size { get; set; } = 10;
        public virtual int? Page { get; set; } = 0;
    }
}
