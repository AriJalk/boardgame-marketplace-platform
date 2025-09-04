using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingGroupRepository : IListingGroupRepository
	{
		public Task<ListingGroup> AddListingGroupAsync(ListingGroup listingGroup)
		{
			throw new NotImplementedException();
		}

		public Task<bool?> DeleteListingGroup(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingGroup?> GetListingsGroupByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}
	}
}
