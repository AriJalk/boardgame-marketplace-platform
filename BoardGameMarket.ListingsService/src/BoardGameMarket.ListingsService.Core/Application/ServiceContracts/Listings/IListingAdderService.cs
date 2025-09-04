using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	public interface IListingAdderService
	{
		Task<Listing?> AddListingAsync(ListingAddRequest? listingAddRequest);
		Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<ListingAddRequest> listingAddRequests);
	}
}
