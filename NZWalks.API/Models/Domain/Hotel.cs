using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Booking.Com_Clone_API.Models.Domain
{
    public class Hotel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public decimal PricePerNight { get; set; }

        public decimal StarRating { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
       
        // Property to get the URLs of images
        [NotMapped] // This property is not mapped to the database
        public List<string> ImageUrls => Images?.Select(image => image.Url).ToList();

        // Navigation properties

        // Define a one-to-many relationship with Facility entity
        public virtual ICollection<Facility> HotelFacilities { get; set; }

        // Define a one-to-many relationship with Image entity
        public virtual ICollection<Image> Images { get; set; }

        // Define a one-to-many relationship with Addresses entity
        //public virtual ICollection<Image> Addresses { get; set; }

    }

    public class Facility
    {
        [Key]
        public Guid FacilityId { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        // Define foreign key property
        public Guid HotelId { get; set; }

        // Define navigation property to Hotel entity
        public virtual Hotel Hotel { get; set; }
    }

    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
        // Define foreign key property
        public Guid HotelId { get; set; }
        // Define navigation property to Hotel entity
        public virtual Hotel Hotel { get; set; }
    }

    //public class UserAddresses
    //{
    //    [Key]
    //    public Guid AddressId { get; set; }
    //    public Guid UserId { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //    public string Mobile { get; set; }
    //    public bool IsDefault { get; set; }
    //    // Define navigation property to Hotel entity
    //    public virtual Hotel Hotel { get; set; }
    //}
}
