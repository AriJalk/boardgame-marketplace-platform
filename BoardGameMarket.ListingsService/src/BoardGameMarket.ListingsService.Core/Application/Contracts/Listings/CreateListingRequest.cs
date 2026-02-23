using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
	public record CreateListingRequest(
	string Title,
	decimal Price,
	CreateListingItemRequest[] ListingItems,
	string? Details)
	{ }
}
