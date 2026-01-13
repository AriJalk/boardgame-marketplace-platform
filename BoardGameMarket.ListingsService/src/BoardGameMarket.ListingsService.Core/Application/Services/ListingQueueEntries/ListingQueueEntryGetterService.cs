using BoardGameMarket.ListingsService.Core.Application.Helpers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	internal class ListingQueueEntryGetterService : IListingQueueEntryGetterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly RequestCache _requestCache;

		public ListingQueueEntryGetterService(IListingQueueEntryRepository listingQueueEntryRepository, RequestCache requestCache)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_requestCache = requestCache;
		}

		private async Task<IEnumerable<ListingQueueEntry>> GetFromCacheOrDb(Guid key, Func<Guid, Task<IEnumerable<ListingQueueEntry>>> fetchFromDb, Func<Guid, IReadOnlyList<ListingQueueEntry>> fetchFromCache)
		{
			IReadOnlyList<ListingQueueEntry> entriesFromCache = fetchFromCache(key);
			if (entriesFromCache.Count > 0)
			{
				return entriesFromCache;
			}
			IEnumerable<ListingQueueEntry> entriesFromDb = await fetchFromDb(key);
			foreach (ListingQueueEntry entry in entriesFromCache)
			{
				_requestCache.AddListingQueueEntry(entry);
			}
			return entriesFromDb;
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntiresByBuyerUserIdAsync(Guid buyerUserId)
		{
			return await GetFromCacheOrDb(buyerUserId, _listingQueueEntryRepository.GetListingQueueEntriesByBuyerUserId, _requestCache.GetListingQueueEntriesByBuyerId);
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await GetFromCacheOrDb(listingId, _listingQueueEntryRepository.GetListingQueueEntriesByListingIdAsync, _requestCache.GetListingQueueEntriesByListingId);

			return queueEntries.OrderBy(qe => qe.CreatedAt);
		}

		// TODO: extract combination of cache + db
		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			List<ListingQueueEntry> queueEntries = new List<ListingQueueEntry>();
			HashSet<Guid> missingIds = new HashSet<Guid>();
			foreach(Guid listingId in listingIds)
			{
				IReadOnlyList<ListingQueueEntry> listingQueueEntiresFromCache = _requestCache.GetListingQueueEntriesByListingId(listingId);
				if (listingQueueEntiresFromCache.Count > 0)
				{
					queueEntries.AddRange(listingQueueEntiresFromCache);
				}
				else
				{
					missingIds.Add(listingId);
				}
			}
			if (missingIds.Count > 0)
			{
				IEnumerable<ListingQueueEntry> listingQueueEntriesFromDb = await _listingQueueEntryRepository.GetListingQueueEntriesByListingIdsAsync(missingIds);
				queueEntries.AddRange(listingQueueEntriesFromDb);
				foreach (ListingQueueEntry entry in queueEntries)
				{
					_requestCache.AddListingQueueEntry(entry);
				}
			}
			return queueEntries.OrderBy(qe => qe.CreatedAt);
		}

		public async Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId)
		{
			if (_requestCache.TryGetListingQueueEntryById(listingQueueId, out var listingQueueEntry))
			{
				return listingQueueEntry;
			}


			listingQueueEntry = await _listingQueueEntryRepository.GetListingQueueEntryByIdAsync(listingQueueId);
			if (listingQueueEntry != null)
			{
				_requestCache.AddListingQueueEntry(listingQueueEntry);
			}
			return listingQueueEntry;
		}
	}
}
