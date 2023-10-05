using ExamSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class Exam : BaseEntity<Guid>
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public ExamStatusEnum Status { get; set; }

        public List<Question>? Questions { get; set; }
    }
}
