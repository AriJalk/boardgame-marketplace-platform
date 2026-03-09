using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingSoldRepository
	{
		Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId);
		Task<ListingSold?> GetListingSoldByListingIdAsync(Guid listingId);
		Task<ListingSold?> GetListingSoldByListingQueueEntryIdAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId);
		Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByUserIdAsync(Guid userId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate);
		Task<int> AddListingSoldAsync(ListingSold listingSold);
		Task<int> DeleteListingSoldAsync(Guid listingSoldId);
	}
}
