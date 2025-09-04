using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingQueueEntryRepository
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingGameIdAsync(Guid listingGameId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByBuyerUserId(Guid buyerUserId);
		Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task<bool?> DeleteListingQueueEntry(Guid listingQueueEntryId);
	}
}
