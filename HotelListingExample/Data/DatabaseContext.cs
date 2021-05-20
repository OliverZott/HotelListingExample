using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HotelListingExample.Data
{
    // Bridge between Entities and actual database
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {

        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"

                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Island",
                    ShortName = "CI"
                }
            );

            modelBuilder.Entity<Hotel>().HasData(
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
                    CountryId = 1,
                    Rating = 4.0
                }
            );
        }

    }
}
