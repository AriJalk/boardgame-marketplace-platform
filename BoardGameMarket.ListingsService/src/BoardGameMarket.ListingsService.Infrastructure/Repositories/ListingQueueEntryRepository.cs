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
		public Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteListingQueueEntry(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByBuyerUserId(Guid buyerUserId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingItemIdAsync(Guid listingItemId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			throw new NotImplementedException();
		}
	}
}
