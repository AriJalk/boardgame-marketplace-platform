using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	internal class ListingQueueEntryUpdaterService : IListingQueueEntryUpdaterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly ILogger<ListingQueueEntryUpdaterService> _logger;

		public ListingQueueEntryUpdaterService(IListingQueueEntryRepository listingQueueEntryRepository, ILogger<ListingQueueEntryUpdaterService> logger)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_logger = logger;
		}

		public async Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToUpdate)
		{
			listingQueueEntryToUpdate.ModifiedAt = DateTime.UtcNow;
			return await _listingQueueEntryRepository.UpdateListingQueueEntryAsync(listingQueueEntryToUpdate);
		}
	}
}
