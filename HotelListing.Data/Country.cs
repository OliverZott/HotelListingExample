using System.Collections.Generic;

namespace HotelListingExample.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        // used with Repository "includes" parameter
        public virtual IList<Hotel> Hotels { get; set; }
    }
}
