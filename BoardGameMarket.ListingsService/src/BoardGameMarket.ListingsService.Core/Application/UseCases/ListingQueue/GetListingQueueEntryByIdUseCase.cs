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
    public class GetListingQueueEntryByIdUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingReadService _listingReadService;

        public GetListingQueueEntryByIdUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingReadService listingReadService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingReadService = listingReadService;
        }

        public async Task<ListingQueueEntryInfo> ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
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

            if (queueEntry.BuyerUserId != requesterUserId && listing.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException("Buyer/Seller/Requester mismatch");
            }

            return queueEntry.ToListingQueueEntryInfo();
        }
    }
}
