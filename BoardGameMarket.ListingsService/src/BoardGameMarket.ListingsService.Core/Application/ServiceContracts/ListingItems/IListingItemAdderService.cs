using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems
{
	public interface IListingItemAdderService
	{
		Task<ListingItem?> AddListingItemAsync(ListingItemAddRequest? listingItemAddRequest);
		Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItemAddRequest> listingItemAddRequests);
	}
}
