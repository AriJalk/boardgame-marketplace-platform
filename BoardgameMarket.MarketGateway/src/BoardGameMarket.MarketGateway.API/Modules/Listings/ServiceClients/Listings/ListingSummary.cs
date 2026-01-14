
using BoardGameMarket.MarketGateway.API.Modules.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings
{
	public record ListingSummary(
		Guid Id,
		string Title,
		Guid ListingGroupId,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		string? Details)
	{ }
}
