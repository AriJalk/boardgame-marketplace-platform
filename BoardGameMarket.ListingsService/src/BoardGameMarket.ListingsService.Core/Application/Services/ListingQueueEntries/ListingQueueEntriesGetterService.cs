using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	public class ListingQueueEntriesGetterService : IListingQueueEntriesGetterService
	{
		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntiresByBuyerUserIdAsync(Guid buyerUserId)
		{
			throw new NotImplementedException();
		}

		public Task<Queue<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId)
		{
			throw new NotImplementedException();
		}
	}
}
