using Booking.Com_Clone_API.Models.Domain;

namespace Booking.Com_Clone_API.Repositories
{
    public interface ISearchRepository
    {
        IEnumerable<Hotel> Search(
            string destination,
            int adultCount,
            int childCount,
            List<string> types,
            List<decimal> starRatings,
            List<string> selectedFacilities,
            decimal? maxPrice,
            string sortOption
        );

    }
}
