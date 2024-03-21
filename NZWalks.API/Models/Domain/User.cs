using System.ComponentModel.DataAnnotations;

namespace Booking.Com_Clone_API.Models.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationToken { get; set; }
        public bool IsVerified { get; set; }
    }
}
