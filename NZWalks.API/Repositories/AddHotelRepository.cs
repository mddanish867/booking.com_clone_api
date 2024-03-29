using AutoMapper;
using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using NZWalks.API.Data;

namespace Booking.Com_Clone_API.Repositories
{
    public class AddHotelRepository : IAddHotelRepository
    {
        private readonly NZWalksDbContext _context;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _cloudinarySettings;


        public AddHotelRepository(NZWalksDbContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _context = context;
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings.Value;

            var account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        ///<summary>
        ///method to get all hotels
        /// </summary>
        public IEnumerable<HotelDto> GetAllHotels()
        {
            var hotels = _context.Hotels
        .Include(h => h.HotelFacilities) // Include hotel facilities
        .Include(h => h.Images) // Include hotel images
        .ToList();

            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        ///<summary>
        ///method to get hotel details based on id
        /// </summary>
        public HotelDto GetHotelById(Guid id)
        {           
                var hotel = _context.Hotels
                    .Include(h => h.HotelFacilities) // Include hotel facilities
                    .Include(h => h.Images) // Include hotel images
                    .FirstOrDefault(h => h.UserId == id);

                if (hotel == null)
                {
                    throw new KeyNotFoundException("Hotel not found");
                }

                return _mapper.Map<HotelDto>(hotel);
        }

        ///<summary>
        ///method to add hotels
        /// </summary>
        public async Task<Guid> AddHotelAsync(HotelDto hotelDto, List<IFormFile> images)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);

            // Upload images to Cloudinary
            var uploadedImageUrls = await UploadImagesToCloudinaryAsync(images);

            // Convert URLs to Image entities and add them to the hotel's Images collection
            hotel.Images = uploadedImageUrls.Select(url => new Image { Url = url }).ToList();

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel.Id;
        }

        ///<summary>
        ///method to upload image on cloudinary
        /// </summary>
        private async Task<List<string>> UploadImagesToCloudinaryAsync(List<IFormFile> images)
        {
            var uploadedImageUrls = new List<string>();

            foreach (var image in images)
            {
                using (var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                        PublicId = Guid.NewGuid().ToString() // Generate a unique public ID for each uploaded image
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    uploadedImageUrls.Add(uploadResult.Uri.ToString());
                }
            }

            return uploadedImageUrls;
        }

        ///<summary>
        ///method to update hotl record
        /// </summary>
        public void UpdateHotel(Guid id, HotelDto hotelDto)
        {
            var existingHotel = _context.Hotels
                                        .Include(h => h.HotelFacilities)
                                        .Include(h => h.Images) // Include images
                                        .FirstOrDefault(h => h.Id == id);
            if (existingHotel == null)
            {
                throw new KeyNotFoundException("Hotel not found");
            }

            // Update properties of the existing hotel with values from the provided hotelDto
            _mapper.Map(hotelDto, existingHotel);

            // Update the LastUpdated property
            existingHotel.LastUpdated = DateTime.UtcNow;

            // Update the facilities
            existingHotel.HotelFacilities.Clear(); // Remove existing facilities
            if (hotelDto.HotelFacilities != null)
            {
                foreach (var facilityName in hotelDto.HotelFacilities)
                {
                    existingHotel.HotelFacilities.Add(new Facility { Name = facilityName });
                }
            }

            // Update the images
            existingHotel.Images.Clear(); // Remove existing images
            if (hotelDto.ImageUrls != null)
            {
                foreach (var imageUrl in hotelDto.ImageUrls)
                {
                    existingHotel.Images.Add(new Image { Url = imageUrl });
                }
            }

            _context.SaveChanges();
        }

        ///<summary>
        ///method to delete hotel record
        /// </summary>
        public void DeleteHotel(Guid id)
        {           
                var existingHotel = _context.Hotels
                                        .Include(h => h.HotelFacilities)
                                        .Include(h => h.Images) // Include images
                                        .FirstOrDefault(h => h.Id == id);
                if (existingHotel == null)
                {
                    throw new KeyNotFoundException("Hotel not found");
                }

                _context.Hotels.Remove(existingHotel);
                _context.SaveChanges();
        }
    }
}
