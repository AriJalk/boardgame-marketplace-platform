using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueuePriorityEntries
{
	internal class ListingQueuePriorityEntryAdderService : IListingQueuePriorityEntryAdderService
	{
		public Task<ListingQueuePriorityEntry?> AddListingQueuePriorityEntryAsync(Guid listingQueueEntryId, decimal priority)
		{
			throw new NotImplementedException();
		}
	}
}
