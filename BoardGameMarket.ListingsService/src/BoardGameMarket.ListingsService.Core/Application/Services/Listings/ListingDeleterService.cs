using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	public class ListingDeleterService : IListingDeleterService
	{
		public Task<bool> DeleteListingAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}
	}
}
