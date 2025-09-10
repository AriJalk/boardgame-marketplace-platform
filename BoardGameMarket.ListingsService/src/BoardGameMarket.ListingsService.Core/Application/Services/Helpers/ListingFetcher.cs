using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Helpers
{
	internal class ListingFetcher
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingQueuePriorityEntryGetterService _listingQueuePriorityEntryGetterService;
		private readonly IListingGroupGetterService _listingGroupGetterService;


		public async Task<ListingQueueEntry> GetListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			ListingQueueEntry? listingQueueEntry = await _listingQueueEntryGetterService.GetListingQueueEntryByIdAsync(listingQueueEntryId);
			if (listingQueueEntry == null)
			{
				throw new KeyNotFoundException("Listing queue entry not found");
			}
			return listingQueueEntry;
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

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesAsync(Guid listingId)
		{
			return await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId);
		}

		public async Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntries(IEnumerable<ListingQueueEntry> listingQueueEntries)
		{
			return await _listingQueuePriorityEntryGetterService.GetListingQueuePriorityEntriesAsync(listingQueueEntries.Select(qe => qe.Id));
		}

		public async Task<IEnumerable<ListingQueuePriorityEntry>> GetListingQueuePriorityEntires(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> listingQueueEntries = await GetListingQueueEntriesAsync(listingId);
			return await GetListingQueuePriorityEntries(listingQueueEntries);
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

		public async Task<Guid> GetSellerUserIdByListingId(Guid listingId)
		{
			Listing listing = await GetListingAsync(listingId);
			ListingGroup listingGroup = await GetListingGroupByIdAsync(listing.ListingGroupId);
			return listingGroup.SellerUserId;
		}
	}
}
