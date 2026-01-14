using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups
{
	public record CreateListingGroup(
	Guid SellerUserId,
	string? Details,
	string? PickupOptions,
	string Location,
	CreateListing[] Listings)
	{
	}
}
