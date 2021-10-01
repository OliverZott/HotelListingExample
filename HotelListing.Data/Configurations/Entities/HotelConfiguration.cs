using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingExample.Data.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Hotel One",
                    Address = "Address One",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Hotel Two",
                    Address = "Address Two",
                    CountryId = 1,
                    Rating = 3.5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Hotel Three",
                    Address = "Address Three",
                    CountryId = 2,
                    Rating = 4.0
                }
            );
        }
    }
}
