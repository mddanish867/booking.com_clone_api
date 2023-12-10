using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

            public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }
        public async Task<Walk?> GetbyIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk != null)
            {
                existingWalk.Name=walk.Name;
                existingWalk.Description=walk.Description;
                existingWalk.LengthInKm= walk.LengthInKm;
                existingWalk.WalkImaheUrl= walk.WalkImaheUrl;
                existingWalk.RegionId=walk.RegionId;
                existingWalk.DifficultyId=walk.DifficultyId;
                dbContext.SaveChangesAsync();
                return existingWalk;
            }
            else
            {
                return null;
            }
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk); 
            dbContext.SaveChangesAsync();
            return (existingWalk);
        }
    }
}
