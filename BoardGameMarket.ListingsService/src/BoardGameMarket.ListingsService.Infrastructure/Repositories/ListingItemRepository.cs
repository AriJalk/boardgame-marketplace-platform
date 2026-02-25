using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingItemRepository : IListingItemRepository
    {
        public Task<int> AddListingItemAsync(ListingItem listingItem)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingItemAsync(Guid listingItemId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingItemsByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingItemAsync(ListingItem listingItem)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingItemsAsync(IEnumerable<ListingItem> listingItems)
        {
            throw new NotImplementedException();
        }
    }
}
