using System.ComponentModel.DataAnnotations;

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

    }
}
