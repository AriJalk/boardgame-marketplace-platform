using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueuePriorityEntries
{
	internal class ListingQueuePriorityEntryGetterService : IListingQueuePriorityEntryGetterService
	{
		private readonly IListingQueuePriorityEntryRepository _listingQueuePriorityEntryRepository;

		public ListingQueuePriorityEntryGetterService(IListingQueuePriorityEntryRepository listingQueuePriorityEntryRepository)
		{
			_listingQueuePriorityEntryRepository = listingQueuePriorityEntryRepository;
		}

		public async Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntriesAsync(IEnumerable<Guid> listingQueueEntryIds)
		{
			return await _listingQueuePriorityEntryRepository.GetListingQueuePriorityEntriesAsync(listingQueueEntryIds);
		}

		public async Task<ListingQueuePriorityEntry?> GetListingQueuePriorityEntryAsync(Guid listingQueueEntryId)
		{
			return await _listingQueuePriorityEntryRepository.GetListingQueuePriorityEntryAsync(listingQueueEntryId);
		}
	}
}
