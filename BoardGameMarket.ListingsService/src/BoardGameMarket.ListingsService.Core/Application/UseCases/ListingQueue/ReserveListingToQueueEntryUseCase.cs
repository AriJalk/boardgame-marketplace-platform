using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class ReserveListingToQueueEntryUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingWriteService _listingWriteService;

        public ReserveListingToQueueEntryUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingReadService listingReadService, IListingWriteService listingWriteService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingReadService = listingReadService;
            _listingWriteService = listingWriteService;
        }

        public async Task<Guid> ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new KeyNotFoundException();
            }
            listing.Status = ListingStatus.Reserved;
            await _listingWriteService.UpdateListingAsync(listing);
            return listingQueueEntryId;
        }
    }
}
