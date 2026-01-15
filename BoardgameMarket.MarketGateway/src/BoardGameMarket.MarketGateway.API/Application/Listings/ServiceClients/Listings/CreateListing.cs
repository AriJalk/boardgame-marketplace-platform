using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
{
	public record CreateListing(
	string Title,
	Guid ListingGroupId,
	decimal Price,
	CreateListingItem[] ListingItems,
	string? Details)
	{ }
}
