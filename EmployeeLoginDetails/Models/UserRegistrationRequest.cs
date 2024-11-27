using System.ComponentModel.DataAnnotations;

namespace EmployeeLoginDetails.Models
{
    public class UserRegistrationRequest
    {
        [Key]
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
