namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems
{
	public record UpdateListingItemRequest(Guid? Id, Guid CatalogId, string? Details)
	{
	}
}
