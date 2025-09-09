using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueuePriorityEntries
{
	internal class ListingQueuePriorityEntryUpdaterService : IListingQueuePriorityEntryUpdaterService
	{
		public Task<ListingQueuePriorityEntry?> UpdateListingQueuePriorityEntryAsync(Guid listingQueueEntryId, decimal priority)
		{
			throw new NotImplementedException();
		}
	}
}
