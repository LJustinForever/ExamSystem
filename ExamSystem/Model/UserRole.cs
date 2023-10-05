using ExamSystem.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Model
{
    public class UserRole : IdentityRole<Guid>
    {
        [Required]
        public UserTypeEnum UserRoleType { get; set; }
    }
}
