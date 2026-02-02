using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.Listings
{
	public interface IListingWriteService
	{
		Task<Listing?> AddListingAsync(Listing listingToAdd);
		Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listingsToAdd);
		Task<bool> DeleteListingAsync(Guid listingId);
		Task DeleteListingsByListingGroupId(Guid listingGroupId);
		Task<Listing?> UpdateListingAsync(Listing listingToUpdate);
	}
}
