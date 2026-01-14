using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups
{
	public record UpdateListingGroup(Guid Id,
		Guid SellerUserId,
		DateTime CreatedAt,
		DateTime ModifiedAt,
		string? Details,
		string? PickupOptions,
		string Location,
		UpdateListing[] Listings)
	{
	}
}
