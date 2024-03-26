using System.ComponentModel.DataAnnotations;

namespace Booking.Com_Clone_API.Models.DTO
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string VerificationToken { get; set; } = null;
        public bool IsVerified { get; set; }
        public string ResetPasswordToken { get; set; } = null;
        public DateTime? ResetPasswordTokenExpiresAt { get; set; } = null;
    }
}
