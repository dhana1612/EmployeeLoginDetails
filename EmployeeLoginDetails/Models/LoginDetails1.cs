using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeLoginDetails.Models
{
    public class LoginDetails1
    {
        public DateOnly Date { get; set; }
        public TimeSpan FirstCheckIn { get; set; }
        public TimeSpan LastCheckOut { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public string? Status { get; set; }
    }
}
