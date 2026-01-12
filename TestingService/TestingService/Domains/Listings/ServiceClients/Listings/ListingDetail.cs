using TestingService.Domains.Listings.Enums;
using TestingService.Domains.Listings.ServiceClients.ListingItems;

namespace TestingService.Domains.Listings.ServiceClients.Listings
{
	public record ListingDetail(
		Guid Id,
		string Title,
		Guid ListingGroupId,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		IReadOnlyCollection<ListingItemInfo>
		ListingItems,
		string? Details)
	{ }
}
