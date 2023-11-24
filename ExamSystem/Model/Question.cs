using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class Question : BaseEntity<Guid>
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(ExamId))]
        public Exam? Exam { get; set; }
        public Guid ExamId { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
