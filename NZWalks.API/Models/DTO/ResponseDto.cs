namespace Booking.Com_Clone_API.Models.DTO
{
    public class ResponseDto
    {
        public string email { get; set; }
        public Guid user_id { get; set; }
        public UserDto user { get; set; }       
        public string jwtToken { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public int status { get; set; } = 200;
    }
}
