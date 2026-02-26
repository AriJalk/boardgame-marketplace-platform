using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings
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
		string? Details,
		Guid SellerUserId)
	{ }
}
