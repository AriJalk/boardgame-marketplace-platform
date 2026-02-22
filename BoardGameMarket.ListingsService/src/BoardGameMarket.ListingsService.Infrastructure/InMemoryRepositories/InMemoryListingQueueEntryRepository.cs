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

		public Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			_listingQueueEntries.Add(listingQueueEntry);
			return Task.FromResult(listingQueueEntry);
		}
		public Task DeleteListingQueueEntiresByListingIdAsync(Guid listingId)
		{
			int removed = _listingQueueEntries.RemoveAll(qe => qe.ListingId == listingId);
			return Task.CompletedTask;
		}
		public Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			_listingQueueEntries.Remove(_listingQueueEntries.FirstOrDefault(l => l.Id == listingQueueEntryId));
			return Task.FromResult(true);
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

        public Task<IEnumerable<ListingQueueEntry>> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
			HashSet<Guid> updateIds = listingQueueEntries.Select(qe => qe.Id).ToHashSet();
            _listingQueueEntries.RemoveAll(qe => updateIds.Contains(qe.Id));
			_listingQueueEntries.AddRange(listingQueueEntries);
			return Task.FromResult(listingQueueEntries);
        }

        public Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			ListingQueueEntry? originalListingQueueEntry = _listingQueueEntries.FirstOrDefault(qe => qe.Id == listingQueueEntry.Id);
			_listingQueueEntries.Remove(originalListingQueueEntry);
			_listingQueueEntries.Add(listingQueueEntry);
			return Task.FromResult(listingQueueEntry);
		}
	}
}
