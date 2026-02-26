using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations
{
    public record ListingReservationInfo(Guid Id, Guid ListingId, Guid ListingQueueEntryId, DateTime ReservedAt, Guid SellerId, Guid BuyerId)
    {
    }
}
