using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
    public record ListingOwnershipInfo(Guid ListingId, Guid SellerUserId)
    {
    }
}
