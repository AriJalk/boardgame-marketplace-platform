namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems
{
	public record UpdateListingItemRequest(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
