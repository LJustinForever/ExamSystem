using ExamSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class ExamLocation : BaseEntity<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ExamLocationStatusEnum Status { get; set; }
    }
}
