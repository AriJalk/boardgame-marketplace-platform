using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
	public interface IListingQueueEntryReadService
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId);
		Task<ListingQueueEntry?> GetListingQueueEntryByListingIdAndUserId(Guid listingId, Guid userId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesBySubscribedUserIdAsync(Guid buyerUserId);
	}
}
