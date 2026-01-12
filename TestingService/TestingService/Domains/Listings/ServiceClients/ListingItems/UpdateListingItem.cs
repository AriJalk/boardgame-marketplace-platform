namespace TestingService.Domains.Listings.ServiceClients.ListingItems
{
	public record UpdateListingItem(Guid Id, Guid CatalogId, string? Details)
	{
	}
}
