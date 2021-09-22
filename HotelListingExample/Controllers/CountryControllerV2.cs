using HotelListingExample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListingExample.Controllers
{
    //[ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryControllerV2 : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly DbSet<Country> _dbSet;

        public CountryControllerV2(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = _databaseContext.Set<Country>();
        }


        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _dbSet.AsNoTracking().ToListAsync();
            return Ok(countries);
        }


        //[HttpGet]
        //public async Task<IActionResult> GetShortCountries()
        //{
        //    var countries = await _dbSet.AsNoTracking().ToListAsync();
        //    return Ok(countries);
        //}


        // Difference to other controller version:
        //      - No usage of repository
        //      - returning entity without mapping on DTO!
        //
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            IQueryable<Country> query = _dbSet;
            query = query.Where(c => c.Id == id);

            var country = await query.AsNoTracking().FirstOrDefaultAsync();
            //var country = await query.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            return Ok(country);
        }
    }
}
