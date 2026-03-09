using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingSaleRepository
	{
		Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId);
		Task<ListingSale?> GetListingSaleByListingIdAsync(Guid listingId);
		Task<ListingSale?> GetListingSaleByListingQueueEntryIdAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId);
		Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId);
		Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId);
		Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate);
		Task<int> AddListingSaleAsync(ListingSale listingSale);
		Task<int> DeleteListingSaleAsync(Guid listingSaleId);
	}
}
