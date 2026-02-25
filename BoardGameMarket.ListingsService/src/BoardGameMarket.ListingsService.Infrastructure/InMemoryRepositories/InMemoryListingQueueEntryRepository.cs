using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
	internal class InMemoryListingQueueEntryRepository : IListingQueueEntryRepository
	{
		private readonly List<ListingQueueEntry> _listingQueueEntries = new List<ListingQueueEntry>();

		public Task<int> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			_listingQueueEntries.Add(listingQueueEntry);
			return Task.FromResult(1);
		}
		public Task<int> DeleteListingQueueEntiresByListingIdAsync(Guid listingId)
		{
			
			int rows = _listingQueueEntries.RemoveAll(qe => qe.ListingId == listingId);
			return Task.FromResult(rows);
		}
		public Task<int> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			int index = _listingQueueEntries.FindIndex(qe => qe.Id == listingQueueEntryId);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listingQueueEntries.RemoveAt(index);
			return Task.FromResult(1);
		}

		public Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter)
		{
			IEnumerable<ListingQueueEntry> query = _listingQueueEntries;
			if (filter.UserId != null)
			{
				query = query.Where(qe => qe.BuyerUserId == filter.UserId);
			}
			if (filter.ListingIds != null)
			{
				HashSet<Guid> listingIds = filter.ListingIds.ToHashSet();
				query = query.Where(qe => listingIds.Contains(qe.ListingId));
			}
			if (filter.ListingQueueEntryIds != null)
			{
				query = query.Where(qe => filter.ListingQueueEntryIds.Contains(qe.Id));
			}

			return Task.FromResult(query);
		}

		public Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
		{
			return Task.FromResult(_listingQueueEntries.FirstOrDefault(qe => qe.Id == listingQueueEntryId));
		}

        public Task<int> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
			int rows = 0;
			foreach(ListingQueueEntry entry in listingQueueEntries)
			{
				int index = _listingQueueEntries.FindIndex(qe => qe.Id == entry.Id);
				if (index == -1)
					continue;
				_listingQueueEntries[index] = entry;
				rows++;
			}
			return Task.FromResult(rows);
        }

        public Task<int> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
            int index = _listingQueueEntries.FindIndex(qe => qe.Id == listingQueueEntry.Id);
            if (index == -1)
			{ 
                return Task.FromResult(0);
			}
            _listingQueueEntries[index] = listingQueueEntry;
			return Task.FromResult(1);
        }
	}
}
