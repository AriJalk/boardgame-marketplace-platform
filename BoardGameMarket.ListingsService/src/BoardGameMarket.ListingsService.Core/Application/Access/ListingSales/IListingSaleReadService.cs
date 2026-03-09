using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingSales
{
	public interface IListingSaleReadService
	{
		Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId);
		Task<ListingSale?> GetListingSaleByListingId(Guid listingId);
		Task<ListingSale?> GetListingSaleByQueueEntryId(Guid queueEntryId);
		Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId);
		Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate);
		Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId);
		Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId);
	}
}
