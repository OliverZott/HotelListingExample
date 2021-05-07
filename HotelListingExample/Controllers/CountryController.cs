using HotelListingExample.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelListingExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()     // where put await? It's already in the GetAll() method which is called inside!
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                return Ok(countries);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetCountries)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }

        }



    }
}
