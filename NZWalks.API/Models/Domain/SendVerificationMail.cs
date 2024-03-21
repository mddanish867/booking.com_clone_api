namespace Booking.Com_Clone_API.Models.Domain
{
    public class SendVerificationMail
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile> Attachments { get; set; }
    }
}
