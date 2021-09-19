using HotelListingExample.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListingExample.Models
{
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Short country name is too long.")]
        public string ShortName { get; set; }
    }

    public class UpdateCountryDto : CreateCountryDto
    {
        public IList<CreateHotelDto> Hotels { get; set; }
    }

    public class CountryDto : CreateCountryDto
    {
        public int Id { get; set; }

        public IList<CreateHotelDto> Hotels { get; set; }  //So GetCountry by id wont return a hotel with countries with hotels (nested)....
    }
}
