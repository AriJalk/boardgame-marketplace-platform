using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	public interface IListingGetterService
	{
		// Regular listings
		Task<Listing?> GetListingByIdAsync(Guid listingId);
		Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId);

		// Sold listings
		Task<IEnumerable<Listing>> GetListingsSoldBySellerUserIdAsync (Guid sellerUserId);
		Task<IEnumerable<Listing>> GetListingsSoldByBuyerUserIdAsync (Guid buyerUserId);
		Task<IEnumerable<Listing>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate);
	}
}
