using BoardGameMarket.ListingsService.Core.Application.Commands;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases
{
	public class AddListingQueueEntryUseCase
	{
		private readonly ListingOwnershipQuery _listingOwnershipQuery;
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingQueueEntryAdderService _listingQueueEntryAdderService;

		public AddListingQueueEntryUseCase(ListingOwnershipQuery listingOwnershipQuery, IListingGetterService listingGetterService, IListingQueueEntryGetterService listingQueueEntryGetterService, IListingQueueEntryAdderService listingQueueEntryAdderService)
		{
			_listingOwnershipQuery = listingOwnershipQuery;
			_listingGetterService = listingGetterService;
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
			_listingQueueEntryAdderService = listingQueueEntryAdderService;
		}

		public async Task<ListingQueueEntryInfo> ExecuteAsync(Guid listingId, AddListingQueueEntryRequest request)
		{

			Guid sellerUserId = await _listingOwnershipQuery.GetSellerUserIdByListingId(listingId);
			if (request.UserId == sellerUserId)
			{
				throw new InvalidOperationException("Subscriber can't be seller");
			}
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}

			if (listing.Status != ListingStatus.Active)
			{
				throw new InvalidOperationException("Listing is not active");
			}

			ListingQueueEntry? queueEntryFromService = await _listingQueueEntryGetterService.GetListingQueueEntryByListingIdAndUserId(listingId, request.UserId);
			if (queueEntryFromService != null)
			{
				return queueEntryFromService.ToListingQueueEntryInfo();
			}

			CreateListingQueueEntryCommand command = new CreateListingQueueEntryCommand(listingId, request.UserId);

			ListingQueueEntry? queueEntryFromAdd = await _listingQueueEntryAdderService.AddListingQueueEntryAsync(command);
			if (queueEntryFromAdd == null)
			{
				throw new InvalidOperationException("Can't subscribe");
			}
			return queueEntryFromAdd.ToListingQueueEntryInfo();
		}
	}
}
