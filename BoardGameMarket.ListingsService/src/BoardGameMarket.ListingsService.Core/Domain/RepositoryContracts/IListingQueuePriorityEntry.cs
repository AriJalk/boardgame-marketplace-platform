using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingQueuePriorityEntry
	{
		Task<ListingQueuePriorityEntry?> GetListingQueuePriorityEntryAsync(Guid listingQueueEntryId);

		Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntriesAsync(IEnumerable<Guid> listingQueueEntryIds);

		Task<ListingQueuePriorityEntry?> AddListingQueuePriorityEntryAsync(ListingQueuePriorityEntry listingQueuePriorityEntry);

		Task<ListingQueuePriorityEntry?> UpdateListingQueuePriorityEntryAsync(ListingQueuePriorityEntry listingQueuePriorityEntry);

		Task<bool> DeleteListingQueuePriorityEntryAsync(Guid listingQueueEntryId);
	}
}
