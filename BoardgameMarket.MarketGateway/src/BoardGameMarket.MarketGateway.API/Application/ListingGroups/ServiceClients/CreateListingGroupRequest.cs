using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients
{
	public record CreateListingGroupRequest(
	Guid SellerUserId,
	string? Details,
	string? PickupOptions,
	string Location,
	CreateListingRequest[] Listings)
	{
	}
}
