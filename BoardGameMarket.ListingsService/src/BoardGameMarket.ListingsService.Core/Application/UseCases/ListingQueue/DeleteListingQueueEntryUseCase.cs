using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class DeleteListingQueueEntryUseCase
    {
        private readonly ListingOwnershipQuery _listingOwnershipQuery;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;

        public DeleteListingQueueEntryUseCase(ListingOwnershipQuery listingOwnershipQuery, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService)
        {
            _listingOwnershipQuery = listingOwnershipQuery;
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
        }

        public async Task ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            ListingQueueEntry? queueEntry = await _listingQueueEntryReadService.GetListingQueueEntryByIdAsync(listingQueueEntryId);
            if (queueEntry == null)
            {
                return;
            }
            Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
            if (queueEntry.BuyerUserId != requesterUserId && requesterUserId != sellerId)
            {
                throw new UnauthorizedAccessException();
            }

            await _listingQueueEntryWriteService.DeleteListingQueueEntryAsync(listingQueueEntryId);

        }
    }
}
