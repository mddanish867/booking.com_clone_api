using AutoMapper;
using Booking.Com_Clone_API.Models.DTO;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Com_Clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;
        private readonly IMapper _mapper;

        public SearchController(ISearchRepository searchRepository, IMapper mapper)
        {
            _searchRepository = searchRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HotelDto>> SearchHotels(
            [FromQuery] string destination,
            //[FromQuery] DateTime? checkIn,
            //[FromQuery] DateTime? checkOut,
            [FromQuery] int adultCount,
            [FromQuery] int childCount,
            [FromQuery] List<string> types,
            [FromQuery] List<decimal> starRatings,
            [FromQuery] List<string> selectedFacilities,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string sortOption
        )
        {
            var hotels = _searchRepository.Search(destination, adultCount, childCount, types, starRatings, selectedFacilities, maxPrice, sortOption);
            var hotelDtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);
            return Ok(hotelDtos);
        }

    }
}
