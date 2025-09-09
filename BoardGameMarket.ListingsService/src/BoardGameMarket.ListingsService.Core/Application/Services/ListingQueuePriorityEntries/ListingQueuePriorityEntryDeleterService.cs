using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueuePriorityEntries
{
	internal class ListingQueuePriorityEntryDeleterService : IListingQueuePriorityEntryDeleterService
	{
		public Task<bool> DeleteListingQueuePriorityEntry(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}
	}
}
