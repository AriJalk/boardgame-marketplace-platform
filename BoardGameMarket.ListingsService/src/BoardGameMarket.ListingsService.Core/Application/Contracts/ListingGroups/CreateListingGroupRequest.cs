using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
	public record CreateListingGroupRequest(
	Guid SellerUserId,
	string? Details,
	string PickupOptions,
	string Location,
	CreateListingRequest[] Listings)
	{
	}
}
