using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	public interface IListingAdderService
	{
		Task<Listing?> AddListingAsync(Listing listingToAdd);
		Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listingsToAdd);
	}
}
