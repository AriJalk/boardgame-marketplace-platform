using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries
{
	public interface IListingQueueEntryGetterService
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntiresByBuyerUserIdAsync(Guid buyerUserId);
	}
}
