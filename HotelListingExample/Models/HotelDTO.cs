using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListingExample.Models
{
    public class CreateHotelDTO
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Hotel name is too long.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Hotel name is too long.")]
        public string Address { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }

        public CountryDTO Country { get; set; }
    }
}
