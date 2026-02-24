using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
	public record UpdateListingGroupRequest(Guid Id,
		string? Details,
		string? PickupOptions,
		string Location,
		UpdateListingRequest[] Listings)
	{
	}
}
