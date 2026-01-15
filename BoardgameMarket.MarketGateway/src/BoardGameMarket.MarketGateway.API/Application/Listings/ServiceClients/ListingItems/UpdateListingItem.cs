namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems
{
	public record UpdateListingItem(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
