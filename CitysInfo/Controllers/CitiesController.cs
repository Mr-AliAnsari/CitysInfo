using Asp.Versioning;
using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CitysInfo.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 10;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities
            (string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var result = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination",
                 JsonSerializer.Serialize(result.Metadata));

            //return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(result.Items));
            return Ok(result.Items);

            //var result = cityEntities.Select(city => new CityWithoutPointOfInterestDto
            //{
            //    Id = city.Id,
            //    Description = city.Description,
            //    Name = city.Name
            //}).ToList();
            //return Ok(result);

            //or

            //var result = new List<CityWithoutPointOfInterestDto>();
            //foreach (var city in cityEntities)
            //{
            //    result.Add(new CityWithoutPointOfInterestDto
            //    {
            //        Id = city.Id,
            //        Description = city.Description,
            //        Name = city.Name
            //    });
            //}
            //return Ok(result);
        }

        /// <summary>
        /// This method returns a specific city based on its ID
        /// </summary>
        /// <param name="cityId">the id of the city to get</param>
        /// <param name="includePointsOfInterest">Will the points of interest be returned ?</param>
        /// <returns>َa city with or without point of interest</returns>
        /// <response code= "200">Return the requested city</response>

        [HttpGet("{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCityId(int cityId, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetByIdForCityAsync(cityId, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (!includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
            }
            return Ok(_mapper.Map<CityDto>(city));

        }
    }
}
