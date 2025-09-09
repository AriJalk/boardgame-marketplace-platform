using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueuePriorityEntries
{
	internal class ListingQueuePriorityEntryGetterService : IListingQueuePriorityEntryGetterService
	{
		public Task<Queue<ListingQueuePriorityEntry>> GetListingQueuePriorityEntriesAsync(IEnumerable<Guid> listingQueueEntryIds)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueuePriorityEntry?> GetListingQueuePriorityEntryAsync(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}
	}
}
