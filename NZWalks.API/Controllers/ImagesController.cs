using Booking.Com_Clone_API.Controllers;
using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ICloudinaryImageRepository imageRepository;
        private readonly ILogger<ImagesController> logger;
        public ImagesController(ICloudinaryImageRepository imageRepository, ILogger<ImagesController> logger)
        {
            this.imageRepository = imageRepository;       
            this.logger = logger;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> imageFiles)
        {
            try
            {
                var uploadedImageUrls = await imageRepository.UploadImagesAsync (imageFiles);
                logger.LogInformation($"Finished hotel retrieval: {JsonSerializer.Serialize(uploadedImageUrls)}");
                return Ok(new { Images = uploadedImageUrls });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
