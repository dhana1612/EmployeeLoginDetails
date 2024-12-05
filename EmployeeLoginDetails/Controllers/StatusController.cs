using EmployeeLoginDetails.Data;
using EmployeeLoginDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


    //Saving Checkin time
        [HttpPost("status")]
        public async Task<IActionResult> Status([FromBody] LoginDetails req)
        {
            try
            {
                _context.EmployeeLoginDetails.Add(req);
                await _context.SaveChangesAsync();


<<<<<<< HEAD
                return Ok();

=======
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



             
        
>>>>>>> 90106cdd9db2cd8496d009daa85755a194d24e70
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }


    //Chart purpose
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



    //Saving CheckOut Time & Working Hour Time
        [HttpPost("CheckOutTimeStore")]
        public async Task<IActionResult> CheckOutTimeStore([FromBody] LoginDetails req)
        {
            try
            {

                var record = _context.EmployeeLoginDetails.FirstOrDefault(r => r.Date == req.Date && r.Username == req.Username && r.CheckIn == req.CheckIn);

                if (record != null)
                {
                    record.CheckOut = req.CheckOut;
                    record.WorkingHours = req.WorkingHours;
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }


    //Saving the Break Time
        [HttpPost("BreakTimeStore")]
        public async Task<IActionResult> BreakTimeStore([FromBody] LoginDetails req)
        {
            try
            {

                var record = _context.EmployeeLoginDetails.FirstOrDefault(r => r.Date == req.Date && r.Username == req.Username && r.CheckOut == req.CheckOut);

                if (record != null)
                {
                    record.Break = req.Break;
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }


    //Checking the Status
        [HttpPost("StatusCheck")]
        public async Task<IActionResult> StatusCheck([FromBody] LoginDetails req)
        {
            try
            {
                string WorkingStatus = GetWorkingStatus(req.WorkingHours);
                return Ok(WorkingStatus); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

    //RetriveWorkStatususingDate
        [HttpPost("GetWorkingStatus")]
        public async Task<IActionResult> GetWorkingStatus([FromBody] LoginDetails req)
        {
            try
            {
                // Fetch records in one query
                var records = _context.EmployeeLoginDetails
                              .Where(record => record.Date == req.Date && record.Username == req.Username)
                              .ToList();

                // Calculate values
                var resultWorkingHours = records.Select(record => record.WorkingHours).ToArray();
                var resultCheckIn = records.Select(record => record.CheckIn).ToArray();
                var resultCheckOut = records.Select(record => record.CheckOut).ToArray();

                var earliestCheckIn = resultCheckIn.Any() ? resultCheckIn.Min() : TimeSpan.Zero;
                var lastCheckOut = resultCheckOut.Any() ? resultCheckOut.Max() : TimeSpan.Zero;
                TimeSpan totalworkingHours = resultWorkingHours.Any() ? resultWorkingHours.Max() : TimeSpan.Zero;

                string WorkingStatus = GetWorkingStatus(totalworkingHours);

                // Create and save LoginDetails1
                LoginDetails1 ld = new LoginDetails1
                {
                    Date = req.Date,
                    FirstCheckIn = earliestCheckIn,
                    LastCheckOut = lastCheckOut,
                    WorkingHours = totalworkingHours,
                    Status = WorkingStatus
                };

                return Ok(ld);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

        private string GetWorkingStatus(TimeSpan totalHours)
        {

            TimeSpan fullDay = TimeSpan.Parse("08:00:00");
            TimeSpan halfDay = TimeSpan.Parse("04:00:00");

            if (totalHours >= fullDay) return "Present";
            if (totalHours >= halfDay) return "HalfDay Present";
            return "Absent";
        }


    }
}
