using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Model
{
    public class Submition : BaseEntity<Guid>
    {
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(ExamTimeId))]
        public ExamTime ExamTime { get; set; }
        public Guid ExamTimeId { get; set; }
    }
}
