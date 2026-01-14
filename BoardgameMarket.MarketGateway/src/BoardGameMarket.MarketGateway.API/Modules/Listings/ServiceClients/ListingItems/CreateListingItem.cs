using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems
{
	public record CreateListingItem(Guid CatalogId, string? Details) { }
}
