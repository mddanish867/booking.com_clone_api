using Booking.Com_Clone_API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using System.Linq;

namespace Booking.Com_Clone_API.Repositories
{
    public class SearchRepository: ISearchRepository
    {
        private readonly NZWalksDbContext _context;

        public SearchRepository(NZWalksDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Hotel> Search(
    string destination,
    int adultCount,
    int childCount,
    List<string> types,
    List<decimal> starRatings,
    List<string> selectedFacilities,
    decimal? maxPrice,
    string sortOption
)
        {
            // Convert destination to uppercase once
            var destinationUpper = destination?.ToUpper();

            // Start with base query
            var query = _context.Hotels.AsQueryable();

            // Apply filters based on the parameters provided
            if (!string.IsNullOrEmpty(destinationUpper))
            {
                query = query.Where(h => h.City.ToUpper() == destinationUpper);
            }

            if (adultCount > 0)
            {
                query = query.Where(h => h.AdultCount >= adultCount);
            }

            if (childCount > 0)
            {
                query = query.Where(h => h.ChildCount >= childCount);
            }

            if (types != null && types.Any())
            {
                query = query.Where(h => types.Contains(h.Type));
            }

            if (starRatings != null && starRatings.Any())
            {
                query = query.Where(h => starRatings.Contains(h.StarRating));
            }

            if (selectedFacilities != null && selectedFacilities.Any())
            {
                query = query.Where(h => h.HotelFacilities != null && h.HotelFacilities.Any(f => selectedFacilities.Contains(f.Name)));
            }
            // Apply filter based on selected price
            if (maxPrice != null)
            {
                query = query.Where(h => h.PricePerNight == maxPrice);
            }
            // Apply sorting based on sortOption
            if (!string.IsNullOrEmpty(sortOption))
            {
                switch (sortOption.ToLower())
                {
                    case "starrating":
                        query = query.OrderByDescending(h => h.StarRating);
                        break;
                    //case "starratingasc":
                    //    query = query.OrderBy(h => h.StarRating);
                    //    break;
                    case "pricepernightdesc":
                        query = query.OrderByDescending(h => h.PricePerNight);
                        break;
                    case "pricepernightasc":
                        query = query.OrderBy(h => h.PricePerNight);
                        break;
                    default:
                        // Default sorting, if no valid sort option provided
                        query = query.OrderBy(h => h.Id);
                        break;
                }
            }
            // Include related entities
            query = query.Include(h => h.HotelFacilities)
                         .Include(h => h.Images);

            // Execute the query and return the results
            return query.ToList();
        }



    }
}
