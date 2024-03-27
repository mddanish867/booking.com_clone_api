using AutoMapper;
using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Controllers;
using NZWalks.API.CustomeActionFilter;
using NZWalks.API.Data;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Booking.Com_Clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UserController> logger;
        private readonly IConfiguration configuration; 

        public UserController(NZWalksDbContext dbContext,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserController> logger,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.configuration = configuration;
        }

        ///<summary>
        ///method to check email already exists
        /// </summary>
        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromBody] UserDto userDto)
        {
            var user = await userRepository.EmailExistAsync(userDto.Email);
            if (user == true)
            {
                // Email exists                
                logger.LogInformation($"Finished email check: {JsonSerializer.Serialize("email alraedy exists")}");

                return Ok("email already exits!");
            }
            else
            {
                // Email does not exist
                logger.LogInformation($"Finished email check: {JsonSerializer.Serialize("email not found!")}");
                return NotFound("email not found");
            }
        }


        ///<summary>
        ///method to create new registration
        /// </summary>
        [HttpPost("register-user")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                // Using Automapper makes much clear code as compare to above method
                var userDomainModel = mapper.Map<User>(userDto);
                userDomainModel = await userRepository.CreateUserAsync(userDomainModel);
                logger.LogInformation($"Finished User created request data: {JsonSerializer.Serialize(userDomainModel)}");
                // Using Automapper makes much clear code as compare to above method
                var regionDtoModel = mapper.Map<UserDto>(userDomainModel);
                return Ok(regionDtoModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Finished User created request data: {JsonSerializer.Serialize("Email already exists")}");
                throw;
            }

        }

        ///<summary>
        ///method to verify mail
        /// </summary>
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                logger.LogInformation($"Finished email and token are required: {JsonSerializer.Serialize("verification failed! email and token are null.")}");
                return BadRequest("Email and token are required");
            }

            var isVerified = await userRepository.VerifyEmailAsync(email, token);
            if (isVerified)
            {
                logger.LogInformation($"Finished verification Successfull: {JsonSerializer.Serialize("Email verified successfully")}");
                return Ok("Email verified successfully");
            }
            else
            {
                logger.LogInformation($"Finished verification failed: {JsonSerializer.Serialize("Invalid email or token")}");
                return BadRequest("Invalid email or token");
            }
                
        }

        ///<summary>
        ///method to login user and generate JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Validate user credentials
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user != null)
                {
                    var decodePassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
                    if (decodePassword)
                    {
                        // Create Token
                        var jwtToken = userRepository.GenerateJwtToken(user);
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
                return BadRequest("Username or password incorrect");
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, "Finished an error occurred during login.");

                // Return a generic error response
                return StatusCode(500, new { message = "Finished an error occurred during login. Please try again later." });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == forgotPassword.Email);
            if (user == null)
            {
                return BadRequest("User with the provided email does not exist.");
            }

            // Generate a password reset token
            userRepository.GenerateResetPasswordToken(user).ToString();

            // Send password reset email
            await userRepository.SendPasswordResetEmailAsync(user.Email, user.ResetPasswordToken);

            return Ok("Password reset email sent successfully.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == resetPassword.Email);
            if (user == null || user.ResetPasswordToken != resetPassword.ResetPasswordToken || user.ResetPasswordTokenExpiresAt < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired password reset token.");
            }

            // Reset the user's password
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPassword.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiresAt = null;
            await dbContext.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }

    }
}
