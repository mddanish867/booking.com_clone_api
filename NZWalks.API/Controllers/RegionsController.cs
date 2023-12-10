﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from dtaabase - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Modes to Dtos
            //var regionDto = new List<RegionDto>();
            //foreach (var region in regionsDomain)
            //{
            //    regionDto.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}

            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method

             var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionDto);
        }

        //Get region by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            var regions = await regionRepository.GetbyIdAsync(id);
            //============  OR  ===================

            //var regions = dbContext.Regions.FirstOrDefault(x => x.Id == id); // only appy for primary key

            if (regions == null)
            {
                return NotFound();
            }

            // Map/Convert region Domain model to Region DTO

            //var regionDto = new RegionDto
            //{
            //    Id = regions.Id,
            //    Code = regions.Code,
            //    Name = regions.Name,
            //    RegionImageUrl = regions.RegionImageUrl,
            //};
            //return Ok(regionDto);

            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method
            return Ok(mapper.Map<RegionDto>(regions));
            
        }

        // Post Create new Region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domai model

            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            //};

            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel =  await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            //return CreatedAtAction(nameof(GetbyId), new { id = regionDto.Id }, regionDto);


            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetbyId), new { id = regionDto.Id }, regionDto);
        }

        // Put Update region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method
            var regionDomainModel =  mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exist
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            //return Ok(regionDto);

            //============================= OR ==========================
            // Using Automapper makes much clear code as compare to above method

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }

        // Delete Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Delete region
            var regionDomainModel =await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted region
            // map Domain Model to DTO

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //return Ok(regionDto);   

            var regionDto = mapper.Map<RegionDto> (regionDomainModel);
            return Ok(regionDto);

        }
    }
}
