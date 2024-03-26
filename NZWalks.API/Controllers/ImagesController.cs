using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ICloudinaryImageRepository imageRepository;
        public ImagesController(ICloudinaryImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;       
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> imageFiles)
        {
            try
            {
                var uploadedImageUrls = await imageRepository.UploadImagesAsync (imageFiles);
                return Ok(new { imageUrls = uploadedImageUrls });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
