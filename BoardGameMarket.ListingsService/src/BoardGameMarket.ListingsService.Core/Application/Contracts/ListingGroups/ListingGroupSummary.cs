using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
	public record ListingGroupSummary(
		Guid Id,
		Guid SellerUserId,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		string? Details,
		string? PickupOptions,
		string Location,
		ListingSummary[] Listings)
	{
	}
}
