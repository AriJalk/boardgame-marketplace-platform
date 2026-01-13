using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems
{
	public record CreateListingItem(Guid CatalogId, string? Details) { }
}
