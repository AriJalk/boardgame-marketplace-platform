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
    public class GetListingQueueEntryByIdUseCase
    {
        private readonly ListingOwnershipQuery _listingOwnershipQuery;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;

        public GetListingQueueEntryByIdUseCase(ListingOwnershipQuery listingOwnershipQuery, IListingQueueEntryReadService listingQueueEntryReadService)
        {
            _listingOwnershipQuery = listingOwnershipQuery;
            _listingQueueEntryReadService = listingQueueEntryReadService;
        }

        public async Task<ListingQueueEntryInfo> ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            ListingQueueEntry? queueEntry = await _listingQueueEntryReadService.GetListingQueueEntryByIdAsync(listingQueueEntryId);

            if (queueEntry == null)
            {
                throw new KeyNotFoundException();
            }

            Guid SellerUserId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
            if (queueEntry.BuyerUserId != requesterUserId && SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException("Buyer/Seller/Requester mismatch");
            }

            return queueEntry.ToListingQueueEntryInfo();
        }
    }
}
