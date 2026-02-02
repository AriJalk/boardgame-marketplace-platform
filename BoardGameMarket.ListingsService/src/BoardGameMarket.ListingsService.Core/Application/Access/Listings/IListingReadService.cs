using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.Listings
{
	public interface IListingReadService
	{
		Task<Listing?> GetListingByIdAsync(Guid listingId);
		Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId);
		Task<IEnumerable<Listing>> GetListingsByIdsAsync(IEnumerable<Guid> listingIds);
	}
}
