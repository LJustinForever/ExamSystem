using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class Answer : BaseEntity<Guid>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question? Question { get; set; }
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        public Guid? UserId { get; set; }

    }
}
