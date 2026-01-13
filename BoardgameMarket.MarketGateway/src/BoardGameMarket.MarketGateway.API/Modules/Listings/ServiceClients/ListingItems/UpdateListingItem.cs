namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems
{
	public record UpdateListingItem(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
