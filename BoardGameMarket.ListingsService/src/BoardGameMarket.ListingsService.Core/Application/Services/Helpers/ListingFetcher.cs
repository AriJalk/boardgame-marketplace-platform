using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Helpers
{
	public class ListingFetcher
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly IListingItemGetterService _listingItemGetterService;

		public ListingFetcher(IListingGetterService listingGetterService, IListingQueueEntryGetterService listingQueueEntryGetterService, IListingGroupGetterService listingGroupGetterService, IListingItemGetterService listingItemGetterService)
		{
			_listingGetterService = listingGetterService;
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
			_listingGroupGetterService = listingGroupGetterService;
			_listingItemGetterService = listingItemGetterService;
		}

		
		public async Task<Listing> GetListingAsync(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}
			return listing;
		}

		public async Task<ListingQueueEntry> GetListingQueueEntryByIdAsync(Guid listingQueuEntryId)
		{
			ListingQueueEntry? listingQueueEntry = await _listingQueueEntryGetterService.GetListingQueueEntryByIdAsync(listingQueuEntryId);
			if (listingQueueEntry == null)
			{
				throw new KeyNotFoundException("Listing queue entry not found");
			}
			return listingQueueEntry;
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesAsyncByListingId(Guid listingId)
		{
			return await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId);
		}

		public async Task<ListingGroup> GetListingGroupByIdAsync(Guid listingGroupId)
		{
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listingGroupId);
			if (listingGroup == null)
			{
				throw new KeyNotFoundException("Listing group not found");
			}
			return listingGroup;
		}

		public async Task<Guid> GetSellerUserIdByGroupIdAsync(Guid listingGroupId)
		{
			ListingGroup listingGroup = await GetListingGroupByIdAsync(listingGroupId);
			return listingGroup.SellerUserId;
		}

		public async Task<Guid> GetSellerUserIdByListingIdAsync(Guid listingId)
		{
			Listing listing = await GetListingAsync(listingId);
			ListingGroup listingGroup = await GetListingGroupByIdAsync(listing.ListingGroupId);
			return listingGroup.SellerUserId;
		}

		public async Task<Guid?> GetBuyerUserIdByListingQueueEntryIdAsync(Guid listingQueueEntryId)
		{
			return (await GetListingQueueEntryByIdAsync(listingQueueEntryId)).BuyerUserId;
		}

		public async Task<IEnumerable<Guid>> GetListingIdsByCatalogId(Guid catalogId)
		{
			IEnumerable<ListingItem> listingItems = await _listingItemGetterService.GetListingItemsByCatalogIdAsync(catalogId);
			return listingItems.Select(i => i.ListingId);
		}

		public async Task<IEnumerable<Listing>> GetListingsByListingIds(IEnumerable<Guid> listingIds)
		{
			return await _listingGetterService.GetListingsByListingIds(listingIds);
		}

		public async Task<HashSet<Guid>> GetListingGroupIdsByCatalogId(Guid catalogId)
		{
			IEnumerable<Guid> listingIds = await GetListingIdsByCatalogId(catalogId);
			IEnumerable<Listing> listings = await GetListingsByListingIds(listingIds);
			return new HashSet<Guid>(listings.Select(l => l.ListingGroupId));
		}
	}
}
