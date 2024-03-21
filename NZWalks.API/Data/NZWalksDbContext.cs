using Booking.Com_Clone_API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext:DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions) 
        {
                
        }
       
        public DbSet<Region> Regions { get; set; }      
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }

        // Only for manual entry to database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            // Seed the data for Region
            // Easy Medium, hard
            var regions = new List<Region>
            {
                new Region()
                {
                    Id = Guid.Parse("89c18c38-1825-4373-9cc2-8f87056db7f0"),
                    Name = "Auckland",
                    Code="AKL",
                    RegionImageUrl="Ak.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("1526dc5d-9bdd-4ac7-aad4-20e49b9eccef"),
                    Name = "Wellington",
                    Code = "WL",
                    RegionImageUrl = "WL.jpg"
                }
               
            };

            // Seed difficulties to database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
