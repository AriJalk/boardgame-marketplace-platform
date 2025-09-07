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
	public class ListingQueueEntryDeleterService : IListingQueueEntryDeleterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly ILogger<ListingQueueEntryDeleterService> _logger;

		public ListingQueueEntryDeleterService(IListingQueueEntryRepository listingQueueEntryRepository, ILogger<ListingQueueEntryDeleterService> logger)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_logger = logger;
		}

		public async Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			_logger.LogInformation($"{nameof(ListingQueueEntryDeleterService)}:{nameof(DeleteListingQueueEntryAsync)}- Deleting Listing queue entry {listingQueueEntryId}");

			return await _listingQueueEntryRepository.DeleteListingQueueEntry(listingQueueEntryId);
		}
	}
}
