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
        public Task<int> AddListingAsync(Listing listing)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListingsAsync(IEnumerable<Listing> listings)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingsByListingGroupIdAsync(Guid listingGroupId)
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

        public Task<int> UpdateListingAsync(Listing listing)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingsAsync(IEnumerable<Listing> listings)
        {
            throw new NotImplementedException();
        }
    }
}
