namespace Booking.Com_Clone_API.Repositories
{
    public interface ICloudinaryImageRepository
    {
        Task<List<string>> UploadImagesAsync(List<IFormFile> imageFiles);
    }
}
