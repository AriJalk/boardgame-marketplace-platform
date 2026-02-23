using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems
{
	public record CreateListingItemRequest(Guid CatalogId, string? Details) { }
}
