using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
	internal class ListingQueuePriorityEntryRepository : IListingQueuePriorityEntryRepository
	{
		public Task<ListingQueuePriorityEntry?> AddListingQueuePriorityEntryAsync(ListingQueuePriorityEntry listingQueuePriorityEntry)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteListingQueuePriorityEntryAsync(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntriesAsync(IEnumerable<Guid> listingQueueEntryIds)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueuePriorityEntry?> GetListingQueuePriorityEntryAsync(Guid listingQueueEntryId)
		{
			throw new NotImplementedException();
		}

		public Task<ListingQueuePriorityEntry?> UpdateListingQueuePriorityEntryAsync(ListingQueuePriorityEntry listingQueuePriorityEntry)
		{
			throw new NotImplementedException();
		}
	}
}
