using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	public class ListingQueueEntriesDeleterService : IListingQueueEntriesDeleterService
	{
		public Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}
	}
}
