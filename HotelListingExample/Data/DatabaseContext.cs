using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListingExample.Data
{
    // Bridge between Entities and actual database
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
