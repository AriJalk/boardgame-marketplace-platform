using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
{
	public record UpdateListing(
		Guid Id,
		string Title,
		decimal Price,
		UpdateListingItem[] ListingItems,
		string? Details)
	{ }
}
