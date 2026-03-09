using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSold
{
    public record ListingSoldInfo(
        Guid Id,
        Guid ListingId,
        DateTime CompletedAt,
        Guid ReservationId,
        Guid ListingQueueEntryId,
        Guid SellerUserId,
        Guid BuyerUserId
        )
    {
    }
}
