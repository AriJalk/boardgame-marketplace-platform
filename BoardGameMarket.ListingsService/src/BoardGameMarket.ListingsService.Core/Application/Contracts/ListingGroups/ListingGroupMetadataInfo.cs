using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
    public record ListingGroupMetadataInfo(
        Guid Id,
        Guid SellerUserId,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        string? Details,
        string PickupOptions,
        string Location)
    {

    }
}
