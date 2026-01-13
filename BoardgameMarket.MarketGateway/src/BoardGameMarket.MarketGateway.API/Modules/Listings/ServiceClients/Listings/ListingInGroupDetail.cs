using BoardGameMarket.MarketGateway.API.Modules.Listings.Enums;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings
{
	public record ListingInGroupDetail(
		Guid Id,
		string Title,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		IReadOnlyCollection<ListingItemInfo>
		ListingItems,
		string? Details)
	{ }
}
