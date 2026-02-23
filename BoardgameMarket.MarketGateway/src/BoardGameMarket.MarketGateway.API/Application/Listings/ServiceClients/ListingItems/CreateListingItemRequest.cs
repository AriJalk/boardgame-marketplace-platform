using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems
{
	public record CreateListingItemRequest(Guid CatalogId, string? Details) { }
}
