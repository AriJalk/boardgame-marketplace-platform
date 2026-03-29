using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class GetListingQueueEntryByUserIdUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingReadService _listingReadService;

        public GetListingQueueEntryByUserIdUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingReadService listingReadService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingReadService = listingReadService;
        }

        public async Task<ListingQueueEntryInfo?> ExecuteAsync(Guid listingId, Guid subscriberUserId, Guid requesterUserId)
        {
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(listingId);
            }

            if (requesterUserId != subscriberUserId && requesterUserId != listing.SellerUserId)
            {
                throw new UnauthorizedAccessException();
            }

            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                ListingIds = [listingId],
                UserId = subscriberUserId,
            };

            ListingQueueEntry? queueEntry = (await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter)).FirstOrDefault();

            return queueEntry?.ToListingQueueEntryInfo();
        }
    }
}
