using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries
{
	internal interface IListingQueueEntryAdderService
	{
		Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToAdd);
	}
}
