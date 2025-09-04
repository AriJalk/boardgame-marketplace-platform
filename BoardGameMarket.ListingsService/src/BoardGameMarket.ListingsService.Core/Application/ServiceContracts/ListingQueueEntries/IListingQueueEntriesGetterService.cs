using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries
{
	public interface IListingQueueEntriesGetterService
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId);
		Task<Queue<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntiresByBuyerUserIdAsync(Guid buyerUserId);
	}
}
