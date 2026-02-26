using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class RemovePriorityFromQueueEntryUseCase
    {
        private readonly ListingOwnershipQuery _listingOwnershipQuery;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;

        public RemovePriorityFromQueueEntryUseCase(ListingOwnershipQuery listingOwnershipQuery, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService)
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
                throw new KeyNotFoundException();
            }
            Guid SellerUserId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
            if (SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }

            queueEntry.Priority = null;

            await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(queueEntry);
        }
    }
}
