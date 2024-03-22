using System.ComponentModel.DataAnnotations;

namespace Booking.Com_Clone_API.Models.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VerificationToken { get; set; } = null;
        public bool IsVerified { get; set; }
        public string ResetPasswordToken { get; set; } = null;
        public DateTime? ResetPasswordTokenExpiresAt { get; set; } = null;
    }
}
