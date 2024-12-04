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

        [HttpPost("status")]
        public async Task<IActionResult> Status([FromBody] LoginDetails req)
        {
            try
            {
                _context.EmployeeLoginDetails.Add(req);
                await _context.SaveChangesAsync();


                return Ok();
        
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }


        [HttpPost("WorkingHoursCalculate")]
        public async Task<IActionResult> WorkingHoursCalculate([FromBody] LoginDetails req)
        {
            try
            {
                var resultWorkingHours = _context.EmployeeLoginDetails
                                     .Where(record => record.Date == req.Date && record.Username == req.Username)
                                     .Select(record => record.WorkingHours)
                                     .ToArray();

                TimeSpan totalworkingHours = resultWorkingHours.Any() ? resultWorkingHours.Max() : TimeSpan.Zero;


                //TimeSpan totalworkingHours = TimeSpan.Zero;

                //foreach (var workingHours in resultWorkingHours)
                //{
                //    totalworkingHours += workingHours;
                //}

                // Output the total time
                Console.WriteLine($"Total Working Hours: {totalworkingHours}");

                TimeSpan expectedTime = TimeSpan.Parse("08:00:00"); // Define the expected total time

                TimeSpan expectedTime1 = TimeSpan.Parse("04:00:00");


                if (totalworkingHours >= expectedTime)
                {
                    return Ok("Present");
                }
                else if (totalworkingHours >= expectedTime1)
                {
                    return Ok("HalfDay Present");
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



        [HttpPost("WorkingHoursStatus")]
        public async Task<IActionResult> WorkingHoursStatus([FromBody] LoginDetails req)
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



                // Determine working status
                TimeSpan expectedTime = TimeSpan.Parse("08:00:00");
                TimeSpan expectedTime1 = TimeSpan.Parse("04:00:00");
                string WorkingStatus = GetWorkingStatus(totalworkingHours, expectedTime, expectedTime1);

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

        private string GetWorkingStatus(TimeSpan totalHours, TimeSpan fullDay, TimeSpan halfDay)
        {
            if (totalHours >= fullDay) return "Present";
            if (totalHours >= halfDay) return "HalfDay Present";
            return "Absent";
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
