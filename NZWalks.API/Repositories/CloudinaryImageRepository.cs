using Booking.Com_Clone_API.Models.Domain;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Booking.Com_Clone_API.Repositories
{
    public class CloudinaryImageRepository : ICloudinaryImageRepository
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _cloudinarySettings;

        public CloudinaryImageRepository(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings.Value;

            var account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }


        public async Task<List<string>> UploadImagesAsync(List<IFormFile> imageFiles)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                throw new ArgumentException("No image files found.");
            }

            var uploadedImageUrls = new List<string>();

            try
            {
                foreach (var file in imageFiles)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            PublicId = Guid.NewGuid().ToString() // Optionally, set a custom public ID for each uploaded image
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        uploadedImageUrls.Add(uploadResult.Uri.ToString());
                    }
                }

                return uploadedImageUrls;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload images: {ex.Message}");
            }
        }
    }
}
