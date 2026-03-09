using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
    public static class ListingReservationMappers
    {
        public static ListingReservationInfo ToListingReservationInfo(this ListingReservation reservation, ListingSummary listing, ListingQueueEntryInfo listingQueueEntry)
        {
            return new ListingReservationInfo(reservation.Id, listing, listingQueueEntry, reservation.CreatedAt, reservation.Status);
        }
    }
}
