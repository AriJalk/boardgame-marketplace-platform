using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations
{
    public class GetListingReservationUseCase
    {
        private readonly IListingReservationReadService _listingReservationReadService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;

        public GetListingReservationUseCase(IListingReservationReadService listingReservationReadService, IListingReadService listingReadService, IListingQueueEntryReadService listingQueueEntryReadService)
        {
            _listingReservationReadService = listingReservationReadService;
            _listingReadService = listingReadService;
            _listingQueueEntryReadService = listingQueueEntryReadService;
        }

        public async Task<ListingReservationInfo> ExecuteAsync(Guid listingReservationId, Guid requesterUserId)
        {
            ListingReservation? reservation = await _listingReservationReadService.GetListingReservationByIdAsync(listingReservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation {listingReservationId}");
            }
            if (reservation.SellerUserId != requesterUserId && reservation.BuyerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }

            Listing? listing = await _listingReadService.GetListingByIdAsync(reservation.ListingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(reservation.ListingId);
            }
            ListingQueueEntry? queueEntry = await _listingQueueEntryReadService.GetListingQueueEntryByIdAsync(reservation.ListingQueueEntryId);
            if (queueEntry == null)
            {
                throw new KeyNotFoundException($"QueueEntry {reservation.ListingQueueEntryId} not found");
            }

            return reservation.ToListingReservationInfo(listing.ToListingSummary(), queueEntry.ToListingQueueEntryInfo());
        }
    }
}
