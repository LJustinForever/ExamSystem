using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ExamSystem.Enums;

namespace ExamSystem.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required, MaxLength(40)]
        public string Name { get; set; }

        [Required, MaxLength(40)]
        public string LastName { get; set; }

        [Required]
        public ApplicationUserStatusEnum Status { get; set; }
    }
}
