using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings
{
	public record CreateListing(
	string Title,
	Guid ListingGroupId,
	decimal Price,
	CreateListingItem[] ListingItems,
	string? Details)
	{ }
}
