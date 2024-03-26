namespace Booking.Com_Clone_API.Models.DTO
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }        
        public string ResetPasswordToken { get; set; } 
    }
}
