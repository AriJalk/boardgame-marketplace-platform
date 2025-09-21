using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueueEntries
{
	internal class AddQueueEntryUseCase
	{
		private readonly ILogger<AddQueueEntryUseCase> _logger;
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingQueueEntryAdderService _listingQueueEntryAdderService;
		private readonly QueueDomainService _queueDomainService;

		public AddQueueEntryUseCase(ILogger<AddQueueEntryUseCase> logger, ListingFetcher listingFetcher, IListingQueueEntryAdderService listingQueueEntryAdderService, QueueDomainService queueDomainService)
		{
			_logger = logger;
			_listingFetcher = listingFetcher;
			_listingQueueEntryAdderService = listingQueueEntryAdderService;
			_queueDomainService = queueDomainService;
		}

		public async Task<ListingQueueEntryResponseDto> ExecuteAsync(ListingQueueEntryAddRequest listingQueueEntryAddRequest, QueueStatus queueStatus = QueueStatus.Queued)
		{
			Guid listingId = listingQueueEntryAddRequest.ListingId;
			Guid buyerId = listingQueueEntryAddRequest.BuyerUserId;
			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingIdAsync(listingId);
			IEnumerable<ListingQueueEntry> queue = await _listingFetcher.GetListingQueueEntriesAsyncByListingId(listingId);

			Result validationResult = _queueDomainService.CanEnqueue(sellerId, buyerId, queue);
			if (!validationResult.IsValid)
			{
				throw new InvalidOperationException(string.Join("\n", validationResult.Errors));
			}

			ListingQueueEntry? listingQueueEntryFromAdd = await _listingQueueEntryAdderService.AddListingQueueEntryAsync(listingQueueEntryAddRequest.ToListingQueueEntry());
			if (listingQueueEntryFromAdd == null)
			{
				throw new InvalidOperationException("Can't add queue entry");
			}
			return listingQueueEntryFromAdd.ToDto();
		}
	}
}
