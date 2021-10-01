using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingExample.Data
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        // In repository the "includes" parameter can be used for included details
        [ForeignKey(nameof(Country))] // Data annotation in []; name of FK is in this case the name of the class
        public int CountryId { get; set; } // hard reference, will be same as the corresponding country id

        public Country Country { get; set; } // this is used to do: "Hotel.Country.PropertyName"
    }
}