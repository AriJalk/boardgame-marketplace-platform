using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations
{
    public record ListingReservationInfo(Guid Id, ListingSummary Listing, ListingQueueEntryInfo ListingQueueEntry, DateTime ReservedAt, ReservationStatus Status)
    {
    }
}
