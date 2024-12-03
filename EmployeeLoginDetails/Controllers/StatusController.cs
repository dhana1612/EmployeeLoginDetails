using EmployeeLoginDetails.Data;
using EmployeeLoginDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public async Task<IActionResult> Status([FromBody] LoginDetails req)
        {
            try
            {
                _context.EmployeeLoginDetails.Add(req);
                await _context.SaveChangesAsync();


                var result = _context.EmployeeLoginDetails
                                     .Where(record => record.Date == req.Date)
                                     .Select(record => record.WorkingHours)
                                     .ToArray();


                Console.WriteLine(string.Join(", ", result));



                TimeSpan totalTime = TimeSpan.Zero; 

                foreach (var workingHours in result) 
                {
                    totalTime += workingHours; 
                }

                // Output the total time
                Console.WriteLine($"Total Working Hours: {totalTime}");

                TimeSpan expectedTime = TimeSpan.Parse("08:00:00"); // Define the expected total time

                if (totalTime >= expectedTime)
                {
                    return Ok("Present");
                }
                else
                {
                    return Ok("Absent");
                }



             
        
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

    }
}
