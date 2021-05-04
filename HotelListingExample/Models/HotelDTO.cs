using System.ComponentModel.DataAnnotations;

namespace HotelListingExample.Models
{
    public class CreateHotelDto
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

    public class HotelDto : CreateHotelDto
    {
        public int Id { get; set; }

        public CountryDto Country { get; set; }
    }
}
