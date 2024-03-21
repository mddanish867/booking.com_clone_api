namespace Booking.Com_Clone_API.Models.Domain
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VerificationLink { get; set; }
    }
}
