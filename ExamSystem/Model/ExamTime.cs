using ExamSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class ExamTime : BaseEntity<Guid>
    {
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public DateTime Time { get; set; }

        [Required]
        public ExamTimeStatusEnum Status { get; set; }

        [ForeignKey(nameof(ExamId))]
        public Exam? Exam { get; set; }
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(ExamLocationId))]
        public ExamLocation? ExamLocation { get; set; }
        public Guid ExamLocationId { get; set; }
    }
}
