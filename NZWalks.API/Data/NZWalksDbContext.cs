using Booking.Com_Clone_API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace NZWalks.API.Data
{
    public class NZWalksDbContext:DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions) 
        {
                
        }
       
        
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }


        // Only for manual entry to database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            

            base.OnModelCreating(modelBuilder);         

        }
    }
}
