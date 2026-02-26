using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class GetListingQueueEntryByUserIdUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public GetListingQueueEntryByUserIdUseCase(IListingQueueEntryReadService listingQueueEntryReadService, ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        public async Task<ListingQueueEntryInfo?> ExecuteAsync(Guid listingId, Guid subscriberUserId, Guid requesterUserId)
        {
            Guid SellerUserId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);

            if (requesterUserId != subscriberUserId && requesterUserId != SellerUserId)
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
