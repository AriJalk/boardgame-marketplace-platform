using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	internal class InMemoryRepository : IListingGroupRepository, IListingRepository, IListingItemRepository, IListingQueueEntryRepository, IListingSoldRepository
	{
		private readonly List<ListingGroup> _listingGroups = new List<ListingGroup>();
		private readonly List<Listing> _listings = new List<Listing>();
		private readonly List<ListingItem> _listingItems = new List<ListingItem>();
		private readonly List<ListingQueueEntry> _listingQueueEntries = new List<ListingQueueEntry>();
		private readonly List<ListingSold> _listingSolds = new List<ListingSold>();
		private readonly ILogger<InMemoryRepository> _logger;

		public InMemoryRepository(ILogger<InMemoryRepository> logger)
		{
			_logger = logger;
		}

		private PagedResult<ListingGroup> GetGroupsPage(List<ListingGroup> orderedListingGroups, Cursor? cursor, int pageSize)
		{
			int start = 0;

			if (cursor != null)
			{
				start = orderedListingGroups.FindIndex(g =>
					g.CreatedAt < cursor.LastCreatedAt || (g.CreatedAt == cursor.LastCreatedAt && g.Id < cursor.LastId));
				if (start < 0) start = orderedListingGroups.Count;
			}

			var pageItems = orderedListingGroups.Skip(start).Take(pageSize).ToList();
			string? nextCursor = null;

			if (pageItems.Count == pageSize)
			{
				var last = pageItems.Last();
				nextCursor = $"{last.CreatedAt:o}|{last.Id}";
				nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(nextCursor));
			}
			return new PagedResult<ListingGroup>
			{
				Items = pageItems,
				NextCursor = nextCursor
			};
		}

		public Task<Listing?> AddListingAsync(Listing listing)
		{
			_listings.Add(listing);
			return Task.FromResult(listing);
		}

		public Task<ListingGroup?> AddListingGroupAsync(ListingGroup listingGroup)
		{
			_listingGroups.Add(listingGroup);
			return Task.FromResult(listingGroup);
		}

		public Task<ListingItem?> AddListingItemAsync(ListingItem listingItem)
		{
			_listingItems.Add(listingItem);
			return Task.FromResult(listingItem);
		}

		public Task<IEnumerable<ListingItem>> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
		{
			_listingItems.AddRange(listingItems);
			return Task.FromResult(listingItems);
		}

		public Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			_listingQueueEntries.Add(listingQueueEntry);
			return Task.FromResult(listingQueueEntry);
		}

		public Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listings)
		{
			_listings.AddRange(listings);
			return Task.FromResult(listings);
		}

		public Task<ListingSold?> AddListingSoldAsync(ListingSold listingSold)
		{
			_listingSolds.Add(listingSold);
			return Task.FromResult(listingSold);
		}

		public Task<bool> DeleteListing(Guid listingId)
		{
			_listings.Remove(_listings.FirstOrDefault(l => l.Id == listingId));
			return Task.FromResult(true);
		}

		public Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			_listingGroups.Remove(_listingGroups.FirstOrDefault(l => l.Id == listingGroupId));
			return Task.FromResult(true);
		}

		public Task<bool> DeleteListingItem(Guid listingItemId)
		{
			_listingItems.Remove(_listingItems.FirstOrDefault(l => l.Id == listingItemId));
			return Task.FromResult(true);
		}

		public Task<bool> DeleteListingQueueEntry(Guid listingQueueEntryId)
		{
			_listingQueueEntries.Remove(_listingQueueEntries.FirstOrDefault(l => l.Id == listingQueueEntryId));
			return Task.FromResult(true);
		}


		public Task<bool> DeleteListingSoldAsync(Guid listingSoldId)
		{
			_listingSolds.Remove(_listingSolds.FirstOrDefault(l => l.Id == listingSoldId));
			return Task.FromResult(true);
		}



		public Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			return Task.FromResult(_listings.FirstOrDefault(l => l.Id == listingId));
		}

		public Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			return Task.FromResult(_listingGroups.FirstOrDefault(l => l.Id == listingGroupId));
		}

		public Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId)
		{
			return Task.FromResult(_listingGroups.Where(l => l.SellerUserId == sellerUserId));
		}

		public Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
		{
			IEnumerable<ListingGroup> query = _listingGroups;
			if (filter != null)
			{
				if (filter.SellerUserId.HasValue)
				{
					query = query.Where(g => g.SellerUserId == filter.SellerUserId.Value);
				}

				if (!string.IsNullOrEmpty(filter.Location))
				{
					query = query.Where(g => g.Location == filter.Location);
				}

				if (filter.ListingGroupIds != null)
				{
					query = query.Where(g => filter.ListingGroupIds.Contains(g.Id));
				}

				/*if (filter.CatalogId.HasValue)
				{
					HashSet<Guid> listingIds = new HashSet<Guid>(
						_listingItems
							.Where(i => i.CatalogId == filter.CatalogId.Value)
							.Select(i => i.ListingId)
					);

					HashSet<Guid> groupIds = new HashSet<Guid>(
						_listings
							.Where(l => listingIds.Contains(l.Id))
							.Select(l => l.ListingGroupId)
					);

					query = query.Where(g => groupIds.Contains(g.Id));
				}*/
			}

			List<ListingGroup> ordered = query.OrderByDescending(g => g.CreatedAt).ThenByDescending(g => g.Id).ToList();
			return Task.FromResult(GetGroupsPage(ordered, cursor, pageSize));
		}

		public Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId)
		{
			return Task.FromResult(_listingItems.FirstOrDefault(i => i.Id == listingItemId));
		}

		public Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listingItems.Where(i => listingIds.Contains(i.ListingId)));
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

		public Task<IEnumerable<Listing>> GetListingsByGroupIdAsync(Guid groupId)
		{
			return Task.FromResult(_listings.Where(l => l.ListingGroupId == groupId));
		}

		public Task<ListingGroup?> GetListingsGroupByListingIdAsync(Guid listingId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingItems.Where(i => i.ListingId == listingId));
		}

		public Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
		{
			return Task.FromResult(_listingItems.Where(i => i.CatalogId == catalogId));
		}

		public Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.Id == listingSoldId));
		}

		public Task<ListingSold?> GetListingSoldByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.ListingId == listingId));
		}

		public Task<ListingSold?> GetListingSoldByListingQueueEntryIdAsync(Guid listingQueueEntryId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.ListingQueueEntryId == listingQueueEntryId));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
		{
			return Task.FromResult(_listingSolds.Where(l => l.BuyerId == buyerUserId));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
		{
			return Task.FromResult(_listingSolds.Where(l => l.CompletedAt >= startDate && l.CompletedAt <= endDate));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
		{
			return Task.FromResult(_listingSolds.Where(l => l.SellerId == sellerUserId));
		}

		public Task<Listing?> UpdateListingAsync(Listing listing)
		{
			Listing? originalListing = _listings.FirstOrDefault(l => l.Id == listing.Id);
			_listings.Remove(originalListing);
			_listings.Add(listing);
			return Task.FromResult(listing);
		}

		public Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroup)
		{
			ListingGroup? originalListingGroup = _listingGroups.FirstOrDefault(lg => lg.Id == listingGroup.Id);
			_listingGroups.Remove(originalListingGroup);
			_listingGroups.Add(listingGroup);
			return Task.FromResult(listingGroup);
		}

		public Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
		{
			ListingQueueEntry? originalListingQueueEntry = _listingQueueEntries.FirstOrDefault(qe => qe.Id == listingQueueEntry.Id);
			_listingQueueEntries.Remove(originalListingQueueEntry);
			_listingQueueEntries.Add(listingQueueEntry);
			return Task.FromResult(listingQueueEntry);
		}

		public Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listings.Where(l => listingIds.Contains(l.Id)));
		}
	}
}
