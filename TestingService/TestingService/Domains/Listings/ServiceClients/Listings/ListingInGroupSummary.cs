using TestingService.Domains.Listings.Enums;

namespace TestingService.Domains.Listings.ServiceClients.Listings
{
	public record ListingInGroupSummary(
		Guid Id,
		string Title,
		ListingStatus Status,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		decimal Price,
		string? Details)
	{ }
}
