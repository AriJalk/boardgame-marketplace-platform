using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Helpers
{
	public class RequestCache
	{
		private readonly Dictionary<Guid, Listing> _listingsById = new Dictionary<Guid, Listing>();
		private readonly Dictionary<Guid, HashSet<Guid>> _listingsByGroupId = new Dictionary<Guid, HashSet<Guid>>();
		private readonly Dictionary<Guid, ListingItem> _listingItemsById = new Dictionary<Guid, ListingItem>();
		private readonly Dictionary<Guid, HashSet<Guid>> _listingItemsByCatalogId = new Dictionary<Guid, HashSet<Guid>>();
		private readonly Dictionary<Guid, HashSet<Guid>> _listingItemsByListingId = new Dictionary<Guid, HashSet<Guid>>();
		private readonly Dictionary<Guid, ListingQueueEntry> _listingQueueEntriesById = new Dictionary<Guid, ListingQueueEntry>();
		private readonly Dictionary<Guid, HashSet<Guid>> _listingQueueEntriesByListingId = new Dictionary<Guid, HashSet<Guid>>();
		private readonly Dictionary<Guid, HashSet<Guid>> _listingQueueEntriesByBuyerUserId = new Dictionary<Guid, HashSet<Guid>>();

		private IReadOnlyList<T> Resolve<T>(HashSet<Guid> ids, Dictionary<Guid, T> map)
		{
			List<T> results = new List<T>(ids.Count);
			foreach (Guid id in ids)
			{
				if (map.TryGetValue(id, out var value))
				{
					results.Add(value);
				}
			}
			return results;
		}

		public void AddListing(Listing listing)
		{
			_listingsById[listing.Id] = listing;

			// Add to group map
			if (!_listingsByGroupId.TryGetValue(listing.ListingGroupId, out var groupListings))
			{
				groupListings = new HashSet<Guid>();
				_listingsByGroupId[listing.ListingGroupId] = groupListings;
			}
			groupListings.Add(listing.Id);
		}

		public void AddListingsByGroupId(Guid listingGroupId, IEnumerable<Listing> listings)
		{
			HashSet<Guid> listingIds = new HashSet<Guid>();
			foreach (Listing listing in listings)
			{
				_listingsById[listing.Id] = listing;
				listingIds.Add(listing.Id);
			}
			_listingsByGroupId[listingGroupId] = listingIds;
		}

		public bool TryGetListingById(Guid listingId, out Listing? listing)
		{
			return _listingsById.TryGetValue(listingId, out listing);
		}

		public IReadOnlyList<Listing> GetListingsByGroupId(Guid groupId)
		{
			if (_listingsByGroupId.TryGetValue(groupId, out var listingIds))
			{
				return Resolve(listingIds, _listingsById);
			}

			return Array.Empty<Listing>();
		}


		public void AddListingItem(ListingItem listingItem)
		{
			_listingItemsById[listingItem.Id] = listingItem;

			// Add to catalog map
			if (!_listingItemsByCatalogId.TryGetValue(listingItem.CatalogId, out var listingItemIds))
			{
				listingItemIds = new HashSet<Guid>();
				_listingItemsByCatalogId[listingItem.CatalogId] = listingItemIds;
			}
			listingItemIds.Add(listingItem.Id);

			// Add to listing map
			if (!_listingItemsByListingId.TryGetValue(listingItem.ListingId, out listingItemIds))
			{
				listingItemIds = new HashSet<Guid>();
				_listingItemsByListingId[listingItem.ListingId] = listingItemIds;
			}
			listingItemIds.Add(listingItem.Id);

		}

		public bool TryGetListingItemById(Guid listingItemId, out ListingItem? listingItem)
		{
			return _listingItemsById.TryGetValue(listingItemId, out listingItem);
		}

		public IReadOnlyList<ListingItem> GetListingItemsByCatalogId(Guid catalogId)
		{
			if (_listingItemsByCatalogId.TryGetValue(catalogId, out var listingItemIds))
			{
				return Resolve(listingItemIds, _listingItemsById);
			}
			return Array.Empty<ListingItem>();
		}

		public IReadOnlyList<ListingItem> GetListingItemsByListingId(Guid listingId)
		{
			if (_listingItemsByListingId.TryGetValue(listingId, out var listingItemIds))
			{
				return Resolve(listingItemIds, _listingItemsById);
			}
			return Array.Empty<ListingItem>();
		}

		public void AddListingQueueEntry(ListingQueueEntry listingQueueEntry)
		{
			_listingQueueEntriesById[listingQueueEntry.Id] = listingQueueEntry;

			// Add to listing map
			if (!_listingQueueEntriesByListingId.TryGetValue(listingQueueEntry.ListingId, out var listingQueueEntries))
			{
				listingQueueEntries = new HashSet<Guid>();
				_listingQueueEntriesByListingId[listingQueueEntry.ListingId] = listingQueueEntries;
			}
			listingQueueEntries.Add(listingQueueEntry.Id);

			// Add to buyer map
			if (!_listingQueueEntriesByBuyerUserId.TryGetValue(listingQueueEntry.BuyerUserId, out listingQueueEntries))
			{
				listingQueueEntries = new HashSet<Guid>();
				_listingQueueEntriesByBuyerUserId[listingQueueEntry.BuyerUserId] = listingQueueEntries;
			}
			listingQueueEntries.Add(listingQueueEntry.Id);
		}

		public bool TryGetListingQueueEntryById(Guid listingQueueEntryId, out ListingQueueEntry? listingQueueEntry)
		{
			return _listingQueueEntriesById.TryGetValue(listingQueueEntryId, out listingQueueEntry);
		}

		public IReadOnlyList<ListingQueueEntry> GetListingQueueEntriesByListingId(Guid listingId)
		{
			if (_listingQueueEntriesByListingId.TryGetValue(listingId, out var listingQueueEntries))
			{
				return Resolve(listingQueueEntries, _listingQueueEntriesById);
			}

			return Array.Empty<ListingQueueEntry>();
		}

		public IReadOnlyList<ListingQueueEntry> GetListingQueueEntriesByBuyerId(Guid buyerId)
		{
			if (_listingQueueEntriesByBuyerUserId.TryGetValue(buyerId, out var listingQueueEntries))
			{
				return Resolve(listingQueueEntries, _listingQueueEntriesById);
			}

			return Array.Empty<ListingQueueEntry>();
		}
	}
}
