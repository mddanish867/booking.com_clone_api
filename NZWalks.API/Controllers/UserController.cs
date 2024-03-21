using AutoMapper;
using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Controllers;
using NZWalks.API.CustomeActionFilter;
using NZWalks.API.Data;

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
        private readonly ILogger<RegionsController> logger;

        public UserController(NZWalksDbContext dbContext,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                // Using Automapper makes much clear code as compare to above method
                var regionDomainModel = mapper.Map<User>(userDto);
                regionDomainModel = await userRepository.CreateUserAsync(regionDomainModel);
                logger.LogInformation($"Finished User created request data: {JsonSerializer.Serialize(regionDomainModel)}");
                // Using Automapper makes much clear code as compare to above method
                var regionDto = mapper.Map<UserDto>(regionDomainModel);
                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return BadRequest("Email and token are required");

            var isVerified = await userRepository.VerifyEmailAsync(email, token);
            if (isVerified)
            {
                logger.LogInformation($"Verification Successfull: {JsonSerializer.Serialize("Email verified successfully")}");
                return Ok("Email verified successfully");
            }
            else
            {
                logger.LogInformation($"Verification failed: {JsonSerializer.Serialize("Invalid email or token")}");
                return BadRequest("Invalid email or token");
            }
                
        }
    }
}
