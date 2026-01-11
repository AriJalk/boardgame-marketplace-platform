namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems
{
	public record ListingItemInfo(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
