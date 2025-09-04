using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	public class ListingAdderService : IListingAdderService
	{
		public Task<Listing?> AddListingAsync(ListingAddRequest? listingAddRequest)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<ListingAddRequest> listingAddRequests)
		{
			throw new NotImplementedException();
		}
	}
}
