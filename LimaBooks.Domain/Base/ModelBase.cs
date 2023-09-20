namespace LimaBooks.Domain.Base
{
    public class ModelBase
    {
        public int? Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
