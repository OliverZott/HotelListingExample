using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelListingExample.Core.DTOs;
using HotelListingExample.Core.Models;
using HotelListingExample.Core.Repository;
using HotelListingExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelListingExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetAll(requestParams); // why await? already in the GetAll()
            var result = _mapper.Map<IList<CountryDto>>(countries);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            // throw new Exception();  / test for global exception handling and logging.
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, q => q.Include(x => x.Hotels));
            var result = _mapper.Map<CountryDto>(country);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto createCountryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}.");
                return BadRequest(ModelState);
            }

            var country = _mapper.Map<Country>(createCountryDto);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            return Accepted("Nice :)");
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto updateCountryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}.\nModel State: {ModelState}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}. No Country with Id {id} found!");
                return BadRequest(ModelState);
            }

            _mapper.Map(updateCountryDto, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"{nameof(DeleteCountry)}: Id is not valid.");
                return BadRequest("Damn, 400 BadRequest i guess");
            }


            var hotel = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"{nameof(DeleteCountry)}: No entity with id {id} was found.");
                return NotFound("Damn, didn't find that country :(");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return Ok($"MUAHAHAHA, did delete country with id {id}.");
        }
    }
}