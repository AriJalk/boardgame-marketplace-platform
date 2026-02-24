using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
{
	public record UpdateListingRequest(
		Guid Id,
		string Title,
		decimal Price,
		UpdateListingItemRequest[] ListingItems,
		string? Details)
	{ }
}
