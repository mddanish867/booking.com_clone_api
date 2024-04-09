namespace Booking.Com_Clone_API.Models.DTO
{
    public class UserAddressDto
    {
        public Guid? AddressId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsDefault { get; set; }
    }
}
