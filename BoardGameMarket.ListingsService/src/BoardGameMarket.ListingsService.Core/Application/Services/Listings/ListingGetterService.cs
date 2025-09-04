using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	public class ListingGetterService : IListingGetterService
	{
		public Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
		{
			throw new NotImplementedException();
		}
	}
}
