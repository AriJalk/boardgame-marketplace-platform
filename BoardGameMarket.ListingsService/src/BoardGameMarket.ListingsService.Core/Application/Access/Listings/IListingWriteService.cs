using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.Listings
{
	public interface IListingWriteService
	{
		Task AddListingAsync(Listing listingToAdd);
		Task AddListingsAsync(IEnumerable<Listing> listingsToAdd);
		Task DeleteListingAsync(Guid listingId);
		Task DeleteListingsByListingGroupId(Guid listingGroupId);
		Task UpdateListingAsync(Listing listingToUpdate);
	}
}
