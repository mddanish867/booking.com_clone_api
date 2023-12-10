using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext:DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) 
        {
                
        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        // Only for manual entry to database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the data for Difficulty
            // Easy Medium, hard
            var difficulties = new List<Difficulty>();
            {
                new Difficulty()
                {
                    Id = Guid.Parse("89c18c38-1825-4373-9cc2-8f87056db7f0"),
                    Name = "Easy"
                };
                new Difficulty()
                {
                    Id = Guid.Parse("1526dc5d-9bdd-4ac7-aad4-20e49b9eccef"),
                    Name = "Medium"
                };
                new Difficulty()
                {
                    Id = Guid.Parse("3c33c606-0f65-448c-9997-36cf8fc380c0"),
                    Name = "Hard"
                };
            };

            // Seed difficulties to database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed the data for Region
            // Easy Medium, hard
            var regions = new List<Region>();
            {
                new Region()
                {
                    Id = Guid.Parse("89c18c38-1825-4373-9cc2-8f87056db7f0"),
                    Name = "Auckland",
                    Code="AKL",
                    RegionImageUrl="Ak.jpg"
                };
                new Region()
                {
                    Id = Guid.Parse("1526dc5d-9bdd-4ac7-aad4-20e49b9eccef"),
                    Name = "Wellington",
                    Code = "WL",
                    RegionImageUrl = "WL.jpg"
                };
               
            };

            // Seed difficulties to database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
