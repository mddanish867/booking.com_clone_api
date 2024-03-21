using System.ComponentModel.DataAnnotations;

namespace Booking.Com_Clone_API.Models.DTO
{
    public class UserDto
    {       
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationToken { get; set; }
        public bool IsVerified { get; set; }
    }
}
