using HotelListingExample.Configurations.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HotelListingExample.Data
{
    // Bridge between Entities and actual database
    // Configuration files can be found in /HotelListingExample/Configurations/... 
    // configs classes either instantiated here directly or registered in Startup.cs for DI
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        private readonly IEntityTypeConfiguration<IdentityRole> _configuration;

        public DatabaseContext(DbContextOptions dbContextOptions, IEntityTypeConfiguration<IdentityRole> configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());

            // tested Service Registration in Startup (although if not necessary here!)
            modelBuilder.ApplyConfiguration(_configuration);
            // alternative with new instantiating
            // modelBuilder.ApplyConfiguration(new RoleConfiguration());

        }

    }
}
