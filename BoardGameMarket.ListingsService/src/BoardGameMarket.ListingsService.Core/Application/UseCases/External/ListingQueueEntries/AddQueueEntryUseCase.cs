using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingQueueEntries
{
	internal class AddQueueEntryUseCase
	{
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingQueueEntryAdderService _listingQueueEntryAdderService;
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ILogger<AddQueueEntryUseCase> _logger;
		private readonly ICurrentUserContext _currentUserContext;

		public AddQueueEntryUseCase(IListingQueueEntryGetterService listingQueueEntryGetterService, IListingQueueEntryAdderService listingQueueEntryAdderService, IListingGetterService listingGetterService, IListingGroupGetterService listingGroupGetterService, ILogger<AddQueueEntryUseCase> logger, ICurrentUserContext currentUserContext)
		{
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
			_listingQueueEntryAdderService = listingQueueEntryAdderService;
			_listingGetterService = listingGetterService;
			_listingGroupGetterService = listingGroupGetterService;
			_logger = logger;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingQueueEntryResponseDto> ExecuteAsync(ListingQueueEntryAddRequest listingQueueEntryAddRequest, QueueStatus? queueStatus = QueueStatus.Queued)
		{
			// Authorize
			IdentityHelper.AuthorizeCurrentUserOrAdmin(listingQueueEntryAddRequest.BuyerUserId, _currentUserContext);

			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingQueueEntryAddRequest.ListingId);
			if (listing == null)
			{
				throw new KeyNotFoundException($"listing {listingQueueEntryAddRequest.ListingId} not found");
			}
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listing.ListingGroupId);
			if (listingGroup == null)
			{
				throw new KeyNotFoundException($"group {listing.ListingGroupId} not found");
			}

			if (listingQueueEntryAddRequest.BuyerUserId == listingGroup.SellerUserId)
			{
				throw new InvalidOperationException($"Buyer Id can't be the same as SellerId");
			}

			Queue<ListingQueueEntry> queue = new Queue<ListingQueueEntry>(await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listing.Id));
			if (queue.Select(q => q.BuyerUserId).Contains(listingQueueEntryAddRequest.BuyerUserId))
			{
				throw new InvalidOperationException("Buyer can't queue more than once");
			}
			ListingQueueEntry? queueEntry = await _listingQueueEntryAdderService.AddListingQueueEntryAsync(listingQueueEntryAddRequest.ToListingQueueEntry());
			if (queueEntry == null)
			{
				throw new InvalidOperationException("Can't create queue entry");
			}

			_logger.LogInformation($"{nameof(AddQueueEntryUseCase)}:{nameof(ExecuteAsync)}- Enqueue successfully");
			return queueEntry.ToDto();
		}
	}
}
