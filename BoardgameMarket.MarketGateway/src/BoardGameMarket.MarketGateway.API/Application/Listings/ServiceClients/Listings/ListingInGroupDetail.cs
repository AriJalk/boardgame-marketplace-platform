using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
{
	public record ListingInGroupDetail(
		Guid Id,
		string Title,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		ListingItemInfo[] ListingItems,
		string? Details)
	{ }
}
