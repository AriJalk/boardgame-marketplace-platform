using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingSoldRepository
	{
		Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId);
		Task<ListingSold?> GetListingSoldByListingIdAsync(Guid listingId);
		Task<ListingSold?> GetListingSoldByListingQueueEntryIdAsync(Guid listingQueueEntryId);
		Task<ListingSold?> AddListingSoldAsync(ListingSold listingSold);
		Task<bool?> DeleteListingSold(Guid listingSoldId);
	}
}
