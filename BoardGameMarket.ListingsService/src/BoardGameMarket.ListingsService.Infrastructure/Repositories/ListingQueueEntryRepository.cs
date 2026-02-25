using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingQueueEntryRepository : IListingQueueEntryRepository
    {
        public Task<int> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingQueueEntiresByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
        {
            throw new NotImplementedException();
        }
    }
}
