using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
	internal class ListingQueueEntryReadService : IListingQueueEntryReadService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingQueueEntryReadService(IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter)
		{
			return await _listingQueueEntryRepository.GetFilteredListingQueueEntriesAsync(filter);
		}

		public async Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
		{
			return await _listingQueueEntryRepository.GetListingQueueEntryByIdAsync(listingQueueEntryId);
		}
	}
}
