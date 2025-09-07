using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings
{
	public interface IListingUpdaterService
	{
		Task<Listing?> UpdateListingAsync(ListingUpdateRequest? listingUpdateRequest);
	}
}
