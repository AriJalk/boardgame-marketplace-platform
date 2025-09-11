using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	internal interface IListingAdderService
	{
		Task<Listing?> AddListingAsync(Listing listingToAdd);
		Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listingsToAdd);
	}
}
