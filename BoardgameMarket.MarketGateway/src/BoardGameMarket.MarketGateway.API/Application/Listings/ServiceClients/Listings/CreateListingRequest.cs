using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
{
	public record CreateListingRequest(
	string Title,
	decimal Price,
	CreateListingItemRequest[] ListingItems,
	string? Details)
	{ }
}
