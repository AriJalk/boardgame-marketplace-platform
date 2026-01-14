using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings
{
	public record UpdateListing(
		Guid Id,
		string Title,
		decimal Price,
		UpdateListingItem[] ListingItems,
		string? Details)
	{ }
}
