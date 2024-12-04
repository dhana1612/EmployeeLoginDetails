using EmployeeLoginDetails.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLoginDetails.Data
{
    public class UserLoginDbContext : DbContext
    {
        public UserLoginDbContext(DbContextOptions<UserLoginDbContext> dbContext) : base(dbContext)
        {

        }

        public DbSet<UserRegistrationRequest> UserLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite unique constraint on Username and Email
            modelBuilder.Entity<UserRegistrationRequest>()
                .HasIndex(u => new { u.UserID, u.Email })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoginDetails> EmployeeLoginDetails { get; set; }

    }
}
