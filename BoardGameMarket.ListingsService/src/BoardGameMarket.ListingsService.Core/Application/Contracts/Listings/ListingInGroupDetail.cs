using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
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
