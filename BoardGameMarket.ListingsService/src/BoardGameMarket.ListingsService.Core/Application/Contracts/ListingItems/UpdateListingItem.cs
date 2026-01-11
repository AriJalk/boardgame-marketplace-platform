namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems
{
	public record UpdateListingItem(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
