using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
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
