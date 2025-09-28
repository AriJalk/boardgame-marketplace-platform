using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries
{
	public interface IListingQueueEntryDeleterService
	{
		Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId);
		Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId);
	}
}
