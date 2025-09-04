using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupGetterService : IListingGroupGetterService
	{
		public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserIdAsync(Guid sellerUserId)
		{
			throw new NotImplementedException();
		}
	}
}
