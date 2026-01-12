using TestingService.Domains.Listings.Enums;

namespace TestingService.Domains.Listings.ServiceClients.Listings
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
