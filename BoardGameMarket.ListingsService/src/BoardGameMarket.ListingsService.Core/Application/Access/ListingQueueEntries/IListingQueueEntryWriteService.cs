using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
	public interface IListingQueueEntryWriteService
	{
		Task AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry);
		Task DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId);
		Task UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToUpdate);
		Task UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries);
	}
}
