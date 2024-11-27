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

        public DbSet<LoginDetails> EmployeeLoginDetails { get; set; }
    }
}
