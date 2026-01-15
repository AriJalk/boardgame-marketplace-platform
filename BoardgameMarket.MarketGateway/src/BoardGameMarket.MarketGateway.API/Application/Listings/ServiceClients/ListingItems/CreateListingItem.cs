using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems
{
	public record CreateListingItem(Guid CatalogId, string? Details) { }
}
