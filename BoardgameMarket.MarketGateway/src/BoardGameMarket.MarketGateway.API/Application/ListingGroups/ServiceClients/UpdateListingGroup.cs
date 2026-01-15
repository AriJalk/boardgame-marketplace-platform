using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients
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
