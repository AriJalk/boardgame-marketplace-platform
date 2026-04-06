using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class DeleteListingQueueEntryUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IListingReadService _listingReadService;

        public DeleteListingQueueEntryUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IListingReadService listingReadService)
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
                return;
            }
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(listingId);
            }

            if (queueEntry.BuyerUserId != requesterUserId && requesterUserId != listing.SellerUserId)
            {
                throw new UnauthorizedAccessException();
            }

            await _listingQueueEntryWriteService.DeleteListingQueueEntryAsync(listingQueueEntryId);

        }
    }
}
