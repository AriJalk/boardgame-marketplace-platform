using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
	public record ListingGroupDetail(
		Guid Id,
		Guid SellerUserId,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		string? Details,
		string? PickupOptions,
		string Location,
		ListingDetail[] Listings)
	{
	}
}
