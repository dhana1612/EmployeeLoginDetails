using System.ComponentModel.DataAnnotations;

namespace EmployeeLoginDetails.Models
{
    public class LoginDetails
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan CheckOut { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public TimeSpan Break { get; set; }
    }
}
