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

        public List<Answer> Answers { get; set; }
    }
}
