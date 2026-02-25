using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
	public class AddListingQueueEntryUseCase
	{
		private readonly ListingOwnershipQuery _listingOwnershipQuery;
		private readonly IListingReadService _listingReadService;
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
		private readonly IListingQueueEntryWriteService _listingQueueEntryAdderService;

		public AddListingQueueEntryUseCase(ListingOwnershipQuery listingOwnershipQuery, IListingReadService listingReadService, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryAdderService)
		{
			_listingOwnershipQuery = listingOwnershipQuery;
			_listingReadService = listingReadService;
			_listingQueueEntryReadService = listingQueueEntryReadService;
			_listingQueueEntryAdderService = listingQueueEntryAdderService;
		}

		// NOTE: not concurrency safe, need to make atomic in DB
		public async Task<Guid> ExecuteAsync(Guid listingId, Guid requesterUserId)
		{

			Guid sellerUserId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
			if (requesterUserId == sellerUserId)
			{
				throw new InvalidOperationException("Subscriber can't be seller");
			}
			Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}

			if (listing.Status != ListingStatus.Active)
			{
				throw new InvalidOperationException("Listing is not active");
			}

			ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
			{
				UserId = requesterUserId,
				ListingIds = new List<Guid> { listingId }
			};
			ListingQueueEntry? queueEntryFromService = (await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter)).FirstOrDefault();
			if (queueEntryFromService != null)
			{
				return queueEntryFromService.Id;
			}

			ListingQueueEntry listingQueueEntryToAdd = new ListingQueueEntry()
			{
				Id = Guid.NewGuid(),
				ListingId = listingId,
				BuyerUserId = requesterUserId,
				CreatedAt = DateTime.UtcNow,
				ModifiedAt = DateTime.UtcNow,
				Status = QueueStatus.Queued,
				Priority = null,
			};

			await _listingQueueEntryAdderService.AddListingQueueEntryAsync(listingQueueEntryToAdd);

			return listingQueueEntryToAdd.Id;
		}
	}
}
