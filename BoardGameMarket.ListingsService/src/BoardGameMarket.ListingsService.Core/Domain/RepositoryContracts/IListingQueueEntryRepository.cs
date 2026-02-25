using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingQueueEntryRepository
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter);
		Task<int> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task<int> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);

		Task<int> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries);
		Task<int> DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task<int> DeleteListingQueueEntiresByListingIdAsync(Guid listingId);
	}
}
