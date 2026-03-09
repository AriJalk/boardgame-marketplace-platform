using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
	public interface IListingSoldReadService
	{
		Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId);
		Task<ListingSold?> GetListingSoldByListingId(Guid listingId);
		Task<ListingSold?> GetListingSoldByQueueEntryId(Guid queueEntryId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate);
		Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByUserIdAsync(Guid userId);
	}
}
