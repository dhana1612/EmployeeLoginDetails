using EmployeeLoginDetails.Data;
using EmployeeLoginDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLoginDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly UserLoginDbContext _context; //Declares a read-only _context variable to interact with the database.

        public StatusController(UserLoginDbContext context)  //Receives an instance of UserLoginDbContext, a database context, through dependency injection. This is used to perform database operations.
        {
            _context = context;
        }
        
        [HttpPost("status")]
        public async Task<IActionResult> status([FromBody] LoginDetails req)    
         
        {
            try
            {
                _context.EmployeeLoginDetails.Add(req);
                await _context.SaveChangesAsync();
                return Ok("Successfully Updated.");
                //return Redirect("DisplayWorkUpdate");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user : {ex.Message}");
            }
        }

        [HttpGet("GetAttendanceSummary")]
        public async Task<IActionResult> GetAttendanceSummary(string username)
        {
            try
            {
                // Assuming EmployeeLoginDetails is a DbSet in UserLoginDbContext.
                var attendanceData = await _context.EmployeeLoginDetails
                    .Where(e => e.Username == username)
                    .GroupBy(e => e.Date) // Grouping by Date
                    .Select(group => new
                    {
                        Date = group.Key,
                        IsPresent = group.Any() // Assuming presence is determined by any record for the day
                    })
                    .ToListAsync();

                int totalDays = attendanceData.Count();
                int daysPresent = attendanceData.Count(a => a.IsPresent);
                int daysAbsent = totalDays - daysPresent;

                return Ok(new { daysPresent, daysAbsent });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

    }
}
