using System.ComponentModel.DataAnnotations;

namespace Booking.Com_Clone_API.Models.DTO
{
    public class LoginDto
    {
        public Guid UserId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
