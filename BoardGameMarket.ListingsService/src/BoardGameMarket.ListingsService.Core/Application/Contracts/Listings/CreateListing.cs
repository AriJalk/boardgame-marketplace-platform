using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
	public record CreateListing(
	string Title,
	Guid ListingGroupId,
	decimal Price,
	IReadOnlyCollection<CreateListingItem>
	ListingItems,
	string? Details)
	{ }
}
