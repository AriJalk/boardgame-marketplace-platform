using TestingService.Domains.Listings.Enums;
using TestingService.Domains.Listings.ServiceClients.ListingItems;

namespace TestingService.Domains.Listings.ServiceClients.Listings
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
