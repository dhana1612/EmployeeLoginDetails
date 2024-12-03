using EmployeeLoginDetails.Data;
using EmployeeLoginDetails.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EmployeeLoginDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserLoginDbContext _context; //Declares a read-only _context variable to interact with the database.
        public UsersController(UserLoginDbContext context)  //Receives an instance of UserLoginDbContext, a database context, through dependency injection. This is used to perform database operations.
        {
            _context = context;
        }


        [HttpPost("register")]                                                 
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            //// Check if the ModelState is valid
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            try

            {
                _context.UserLogin.Add(request);
                await _context.SaveChangesAsync();
                return Ok("NewUser created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user : {ex.Message}");
            }
        }


        //Login  <--UserLogin--> 
        [HttpPost("Login")]
        public async Task<IActionResult> VerifyLoginDetails([FromBody] Dummy res)
        {
            //Search for the member by email

            var member = await _context.UserLogin.FirstOrDefaultAsync(m => m.Email == res.Email);


            // Check if the member exists
            if (member != null)
            {
                bool isMatch = member.Password == res.Password;

                if (isMatch)
                {
                    return Ok("User");
                }
                return NotFound("Email ID and Password do not match.");
            }
            else
            {
                return NotFound("Email ID not found");
            }
        }


        [HttpPost("RetriveUserName")]
        public async Task<IActionResult> RetriveUserName([FromBody] Dummy res)
        {

            // Search for the member by email
            var member = await _context.UserLogin.FirstOrDefaultAsync(m => m.Email == res.Email);


            var username = member.Username;

            // Check if the member exists
            if (member == null)
            {
                return NotFound("Email not found");
            }
            else
            {
                return Ok(username);
            }

        }
    }
}
