using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Model
{
    public class BaseEntity<T>
    {
        [Key]
        public Guid Id { get; set; }
    }
}
