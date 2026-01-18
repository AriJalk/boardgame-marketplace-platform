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

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByBuyerUserId(Guid buyerUserId)
		{
			return Task.FromResult(_listingQueueEntries.Where(qe => qe.BuyerUserId == buyerUserId));
		}
		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingQueueEntries.Where(qe => qe.ListingId == listingId));
		}

		public Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listingQueueEntries.Where(qe => listingIds.Contains(qe.ListingId)));
		}

		public Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
		{
			return Task.FromResult(_listingQueueEntries.FirstOrDefault(qe => qe.Id == listingQueueEntryId));
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
