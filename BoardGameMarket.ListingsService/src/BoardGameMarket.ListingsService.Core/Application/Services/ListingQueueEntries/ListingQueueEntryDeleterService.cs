using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	internal class ListingQueueEntryDeleterService : IListingQueueEntryDeleterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingQueueEntryDeleterService(IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}

		public async Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			await _listingQueueEntryRepository.DeleteListingQueueEntiresByListingIdAsync(listingId);
		}

		public async Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			return await _listingQueueEntryRepository.DeleteListingQueueEntryAsync(listingQueueEntryId);
		}
	}
}
