using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Data
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }
    }
}
