using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	public class ListingQueueEntriesAdderService : IListingQueueEntriesAdderService
	{
		public Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntryAddRequest? listingQueueEntryAddRequest)
		{
			throw new NotImplementedException();
		}
	}
}
