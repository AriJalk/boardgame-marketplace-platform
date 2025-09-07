using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	public class ListingQueueEntryUpdaterService : IListingQueueEntryUpdaterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly ILogger<ListingQueueEntryUpdaterService> _logger;

		public ListingQueueEntryUpdaterService(IListingQueueEntryRepository listingQueueEntryRepository, ILogger<ListingQueueEntryUpdaterService> logger)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_logger = logger;
		}

		public async Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntryUpdateRequest? listingQueueEntriesUpdateRequest)
		{
			ArgumentNullException.ThrowIfNull(listingQueueEntriesUpdateRequest, nameof(listingQueueEntriesUpdateRequest));

			_logger.LogInformation($"{nameof(ListingQueueEntryUpdaterService)}:{nameof(UpdateListingQueueEntryAsync)}- Update listing queue {listingQueueEntriesUpdateRequest.Id}");

			ListingQueueEntry? listingQueueEntryToUpdate = await _listingQueueEntryRepository.GetListingQueueEntryByIdAsync(listingQueueEntriesUpdateRequest.Id);

			if (listingQueueEntryToUpdate == null)
			{
				return null;
			}

			if (listingQueueEntryToUpdate.ListingId != listingQueueEntriesUpdateRequest.ListingId || listingQueueEntryToUpdate.BuyerUserId != listingQueueEntriesUpdateRequest.BuyerUserId)
			{
				throw new ArgumentException("Mismatch update info");
			}

			listingQueueEntryToUpdate.Status = listingQueueEntriesUpdateRequest.Status;
			listingQueueEntryToUpdate.ModifiedAt = DateTime.UtcNow;

			return await _listingQueueEntryRepository.UpdateListingQueueEntryAsync(listingQueueEntryToUpdate);
		}
	}
}
