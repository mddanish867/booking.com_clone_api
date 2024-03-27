using System;
using System.Collections.Generic;

namespace Booking.Com_Clone_API.Models.DTO
{
    public class HotelDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal StarRating { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public List<string> HotelFacilities { get; set; }
        public List<string> ImageUrls { get; set; }
    }

    
}
