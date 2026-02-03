using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
	public interface IListingQueueEntryReadService
	{
		Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId);
		Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter);
	}
}
