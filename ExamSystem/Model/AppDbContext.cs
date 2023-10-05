using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Model
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, UserRole, Guid>
    {
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<ExamLocation> ExamLocation { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamTime> ExamTime { get; set; }
        public DbSet<Submition> Submition { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
