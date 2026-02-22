using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingQueueEntryRepository
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter);
		Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);

		Task<IEnumerable<ListingQueueEntry>> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries);
		Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task DeleteListingQueueEntiresByListingIdAsync(Guid listingId);
	}
}
