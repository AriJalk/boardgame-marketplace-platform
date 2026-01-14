using BoardGameMarket.MarketGateway.API.Modules.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings
{
	public record ListingDetail(
		Guid Id,
		string Title,
		Guid ListingGroupId,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		ListingItemInfo[] ListingItems,
		string? Details)
	{ }
}
