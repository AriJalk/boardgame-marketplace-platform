using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingRepository : IListingRepository
	{
		public Task<Listing?> AddListingAsync(Listing listing)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listings)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteListingAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task DeleteListingsByListingGroupIdAsync(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}

		public Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListings()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsByGroupIdAsync(Guid groupId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			throw new NotImplementedException();
		}

		public Task<Listing?> UpdateListingAsync(Listing listing)
		{
			throw new NotImplementedException();
		}

        public Task<IEnumerable<Listing>> UpdateListingsAsync(IEnumerable<Listing> listings)
        {
            throw new NotImplementedException();
        }
    }
}
