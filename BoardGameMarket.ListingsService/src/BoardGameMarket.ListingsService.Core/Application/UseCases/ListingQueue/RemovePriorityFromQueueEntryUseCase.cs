using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
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
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IListingReadService _listingReadService;

        public RemovePriorityFromQueueEntryUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IListingReadService listingReadService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _listingReadService = listingReadService;
        }

        public async Task ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            ListingQueueEntry? queueEntry = await _listingQueueEntryReadService.GetListingQueueEntryByIdAsync(listingQueueEntryId);
            if (queueEntry == null)
            {
                throw new KeyNotFoundException();
            }
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(listingId);
            }
            if (listing.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }

            queueEntry.Priority = null;

            await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(queueEntry);
        }
    }
}
