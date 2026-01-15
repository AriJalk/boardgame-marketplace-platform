using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients
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
