using AutoMapper;
using HotelListingExample.Data;
using HotelListingExample.IRepository;
using HotelListingExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetCountries() // why await? already in the GetAll()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var result = _mapper.Map<IList<CountryDto>>(countries);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetCountries)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDto>(country);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetCountry)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto createCountryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}.");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(createCountryDto);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                return Accepted("Nice :)");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(CreateCountry)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto updateCountryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}.\nModel State: {ModelState}");
                return BadRequest(ModelState);
            }

            try
            {
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
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(UpdateCountry)}. Error message: {e.Message}");
                return StatusCode(500, $"Internal Server Error. Problem in {nameof(UpdateCountry)} Sorry :)");
            }
        }
    }
}