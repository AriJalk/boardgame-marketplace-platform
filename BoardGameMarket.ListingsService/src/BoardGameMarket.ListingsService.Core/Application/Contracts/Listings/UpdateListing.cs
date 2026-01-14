using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.Listings
{
	public record UpdateListing(
		Guid Id,
		string Title,
		decimal Price,
		UpdateListingItem[] ListingItems,
		string? Details)
	{ }
}
