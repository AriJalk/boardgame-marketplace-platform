using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	public class ListingQueueEntryAdderService : IListingQueueEntryAdderService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly ILogger<ListingQueueEntryAdderService> _logger;

		public ListingQueueEntryAdderService(IListingQueueEntryRepository listingQueueEntryRepository, ILogger<ListingQueueEntryAdderService> logger)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_logger = logger;
		}

		public async Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntryAddRequest? listingQueueEntryAddRequest)
		{
			ArgumentNullException.ThrowIfNull(listingQueueEntryAddRequest, nameof(listingQueueEntryAddRequest));

			_logger.LogInformation($"{nameof(ListingQueueEntryAdderService)}:{nameof(AddListingQueueEntryAsync)}- Adding Listing queue");

			ListingQueueEntry listingQueueEntryToAdd = listingQueueEntryAddRequest.ToListingQueueEntry();
			listingQueueEntryToAdd.Id = Guid.NewGuid();
			listingQueueEntryToAdd.CreatedAt = DateTime.UtcNow;
			listingQueueEntryToAdd.ModifiedAt = DateTime.UtcNow;

			return await _listingQueueEntryRepository.AddListingQueueEntryAsync(listingQueueEntryToAdd);
		}
	}
}
