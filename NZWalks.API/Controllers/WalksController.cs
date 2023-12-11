using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomeActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                // Map Dto to Domain model
                var walksDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walksDomainModel);

                // Map Domain Model to Dto
                return Ok(mapper.Map<WalkDto>(walksDomainModel));
           
        }

        //Get : api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 1000)
        {
            // Map Dto to Domain model
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery,sortBy,isAscending ?? true, pageNumber,pageSize);

            // Map Domain Model to Dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetbyIdAsync([FromRoute] Guid id)
        {
            // Map Dto to Domain model
            var walksDomainModel = await walkRepository.GetbyIdAsync(id);
            if(walksDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
             // Map Dto to Domain model
                var walksDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walksDomainModel = await walkRepository.UpdateAsync(id, walksDomainModel);
                if (walksDomainModel == null)
                {
                    return NotFound();
                }

                // Map Domain Model to Dto
                return Ok(mapper.Map<WalkDto>(walksDomainModel));
           
            
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var deletedWalksDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalksDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(deletedWalksDomainModel));
        }
    }
}
