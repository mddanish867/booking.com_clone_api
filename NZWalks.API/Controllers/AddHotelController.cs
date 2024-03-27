using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Booking.Com_Clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddHotelController : ControllerBase
    {
        private readonly IAddHotelRepository _hotelRepository;
        private readonly ILogger<AddHotelController> _logger;
        public AddHotelController(IAddHotelRepository hotelRepository, ILogger<AddHotelController> logger)
        {
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        ///<summary>
        ///endpoint to get all hotel
        /// </summary>
        [HttpGet("get-all-hotel")]
        public ActionResult<IEnumerable<HotelDto>> GetAllHotels()
        {
            try
            {
                var hotels = _hotelRepository.GetAllHotels();                          
                _logger.LogInformation($"Finished hotel retrieval: {JsonSerializer.Serialize(hotels)}");
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        ///<summary>
        ///endpoint to get hotel based on id
        /// </summary>
        [HttpGet("get-hotel/{id}")]
        public ActionResult<HotelDto> GetHotelById(Guid id)
        {
            try
            {
                var hotel = _hotelRepository.GetHotelById(id);
                _logger.LogInformation($"Finished hotel retrieval based on id: {JsonSerializer.Serialize(hotel)}");
                return Ok(hotel);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation($"Retrieval failed: {JsonSerializer.Serialize("Hotel not found")}");
                return NotFound("Hotel not found");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        ///<summary>
        ///endpoint to add hotel
        /// </summary>
        [HttpPost("add-hotel")]
        public async Task<ActionResult<Guid>> AddHotel([FromForm] HotelDto hotelDto, [FromForm] List<IFormFile> images)
        {
            try
            {
                if (hotelDto == null || images == null || images.Count == 0)
                {
                    _logger.LogInformation($"Paramters are null: {JsonSerializer.Serialize("Hotel data and at least one image file are required")}");

                    return BadRequest("Hotel data and at least one image file are required");
                }

                // Call repository method to add hotel
                var hotelId = await _hotelRepository.AddHotelAsync(hotelDto, images);
                _logger.LogInformation($"Hotel added successfully: {JsonSerializer.Serialize(hotelId)}");
                return CreatedAtAction(nameof(GetHotelById), new { id = hotelId }, hotelId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        ///<summary>
        ///endpoint to update hotel
        /// </summary>
        [HttpPut("update-hotel/{id}")]
        public ActionResult UpdateHotel(Guid id, [FromForm] HotelDto hotelDto)
        {
            try
            {
                if (hotelDto == null)
                {
                    _logger.LogInformation($"Paramters are null: {JsonSerializer.Serialize("Hotel parameter is required")}");
                    return BadRequest("Hotel data is required");
                }              
                _hotelRepository.UpdateHotel(id, hotelDto);
                _logger.LogInformation($"Hotel updated successfully: {JsonSerializer.Serialize(id)}");

                return Ok(new { id, message = "Hotel details updated successfully" }); // Return 200 OK with the updated hotel's ID
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation($"Something went wrong:: {JsonSerializer.Serialize("Hotel not found")}");
                return NotFound("Hotel not found");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        ///<summary>
        ///endpoint to delete hotel
        /// </summary>
        [HttpDelete("remove-hotel/{id}")]
        public ActionResult DeleteHotel(Guid id)
        {
            try
            {
                _hotelRepository.DeleteHotel(id);
                _logger.LogInformation($"Hotel deleted successfully: {JsonSerializer.Serialize(id)}");
                return Ok(new { id, message = "Hotel deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation($"Something went wrong:: {JsonSerializer.Serialize("Hotel not found")}");
                return NotFound(new { id, message = "Hotel not found" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Internal Server Error:: {JsonSerializer.Serialize(ex.Message)}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
