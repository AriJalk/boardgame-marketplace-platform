using BoardGameMarket.ListingsService.Core.Application.Commands;
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
		Task<ListingQueueEntry?> AddListingQueueEntryAsync(CreateListingQueueEntryCommand command);
		Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId);
		Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToUpdate);
		Task<IEnumerable<ListingQueueEntry>> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries);
	}
}
