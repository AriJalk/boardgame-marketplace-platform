using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	public interface IListingGetterService
	{
		Task<Listing?> GetListingByIdAsync(Guid listingId);
		Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId);
	}
}
