using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingQueueEntryRepository
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId);
		//Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingItemIdAsync(Guid listingItemId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesBySubscribedUserId(Guid buyerUserId);
		Task<ListingQueueEntry?> GetListingQueueEntryByListingIdAndBuyerUserId(Guid listingId,  Guid buyerUserId);
		Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task DeleteListingQueueEntiresByListingIdAsync(Guid listingId);
	}
}
