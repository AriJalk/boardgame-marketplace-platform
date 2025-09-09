using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries
{
	internal interface IListingQueuePriorityEntryUpdaterService
	{
		Task<ListingQueuePriorityEntry?> UpdateListingQueuePriorityEntryAsync(Guid listingQueueEntryId, decimal priority);
	}
}
