using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries
{
	public interface IListingQueuePriorityEntryGetterService
	{
		Task<ListingQueuePriorityEntry?> GetListingQueuePriorityEntryAsync(Guid listingQueueEntryId);
		Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntriesAsync(IEnumerable<Guid> listingQueueEntryIds);
	}
}
