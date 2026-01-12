using TestingService.Domains.Listings.ServiceClients.ListingItems;

namespace TestingService.Domains.Listings.ServiceClients.Listings
{
	public record UpdateListing(
		Guid Id,
		string Title,
		decimal Price,
		IReadOnlyCollection<UpdateListingItem> ListingItems,
		string? Details)
	{ }
}
