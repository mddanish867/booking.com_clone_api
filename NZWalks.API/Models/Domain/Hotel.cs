using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Booking.Com_Clone_API.Models.Domain
{
    public class Hotel
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        [NotMapped]
        public Facilities HotelFacilities { get; set; } // Complex type for facilities
        // Serialized property for storing facilities in the database
        public string FacilitiesJson
        {
            get => HotelFacilities != null ? JsonSerializer.Serialize(HotelFacilities) : null;
            set => HotelFacilities = value != null ? JsonSerializer.Deserialize<Facilities>(value) : null;
        }
        public decimal PricePerNight { get; set; }
        public decimal StarRating { get; set; }
        [NotMapped]
        public Images ImageUrl { get; set; }
        // Serialized property for storing facilities in the database
        public string ImageUrlJson
        {
            get => ImageUrl != null ? JsonSerializer.Serialize(HotelFacilities) : null;
            set => ImageUrl = value != null ? JsonSerializer.Deserialize<Images>(value) : null;
        }
        public DateTime LastUpdated { get; set; }

        // Define a nested class for Facilities
        public class Facilities
        {
            public List<string> FacilityList { get; set; }
        }
        // Define a nested class for Facilities
        public class Images
        {
            public List<string> ImageUrls { get; set; }
        }
    }
}
