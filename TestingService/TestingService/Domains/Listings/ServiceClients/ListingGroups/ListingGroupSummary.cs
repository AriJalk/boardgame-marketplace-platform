using TestingService.Domains.Listings.ServiceClients.Listings;

namespace TestingService.Domains.Listings.ServiceClients.ListingGroups
{
	public record ListingGroupSummary(
		Guid Id,
		Guid SellerUserId,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		string? Details,
		string? PickupOptions,
		string Location,
		IReadOnlyCollection<ListingInGroupSummary> Listings)
	{
	}
}
