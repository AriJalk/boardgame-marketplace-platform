using System.Text.Json;
using TestingService.Dtos.Item;
using TestingService.Dtos.Listing;
using TestingService.Dtos.ListingGroup;
using TestingService.Dtos.Queue;
using TestingService.Dtos.User;

namespace TestingService.Services
{

	public class ListingsService
	{
		private Random _random = new Random();
		public Dictionary<Guid, ListingGroupDetailDto> ListingGroups = new Dictionary<Guid, ListingGroupDetailDto>();
		public Dictionary<Guid, ListingDetailDto> Listings = new Dictionary<Guid, ListingDetailDto>();
		public Dictionary<Guid, List<ListingQueueEntryResponseDto>> QueueEntries = new Dictionary<Guid, List<ListingQueueEntryResponseDto>>();
		public Dictionary<Guid, UserSummaryDto> Users = new Dictionary<Guid, UserSummaryDto>();

		public ListingsService(IWebHostEnvironment hostEnvironment)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "MOCK_DATA.json"));

			IEnumerable<ListingGroupDetailDto>? listingGroups = JsonSerializer.Deserialize<IEnumerable<ListingGroupDetailDto>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

			if (listingGroups == null)
				return;
			foreach (ListingGroupDetailDto listingGroup in listingGroups)
			{
				BuildListingGroup(listingGroup);
			}
		}

		private void BuildListingGroup(ListingGroupDetailDto listingGroup)
		{
			ListingGroups[listingGroup.Id] = listingGroup;
			if (listingGroup.Listings == null)
			{
				return;
			}
			foreach (ListingDetailDto listing in listingGroup.Listings)
			{
				BuildListing(listing);
			}
		}

		private void BuildListing(ListingDetailDto listing)
		{
			Listings[listing.Id] = listing;
			if (listing.Items == null) 
			{
				return;
			}
			foreach(ListingItemDto item in listing.Items)
			{
				if (item.CatalogItem == null)
				{
					return;
				}
				item.CatalogItem.BggLink = GenerateBggUrl();
			}
		}

		private string GenerateBggUrl()
		{
			int id = _random.Next(1, 461367);
			return $"https://www.boardgamegeek.com/boardgame/{id}";
		}

		public bool UpdateQueuePriority(Guid listingId, Guid queueEntryId, int requestedIndex)
		{
			if (QueueEntries.TryGetValue(listingId, out var listingQueues))
			{
				ListingQueueEntryResponseDto? queueEntry = listingQueues.FirstOrDefault(q => q.Id == queueEntryId);
				if (queueEntry != null)
				{
					int entryIndex = listingQueues.IndexOf(queueEntry);
					var priorityList = listingQueues.Where(q => q.Priority != null).OrderBy(q => q.Priority).ToList();
					if (priorityList.ElementAtOrDefault(requestedIndex) == queueEntry)
					{
						return false;
					}
					var filteredList = priorityList.Where(q => q != queueEntry).ToList();
					if (priorityList.Count == 0)
					{
						queueEntry.Priority = 0;
					}
					else if (requestedIndex == 0)
					{
						queueEntry.Priority = filteredList[0].Priority - 1000;
					}
					else if (requestedIndex >= filteredList.Count - 1)
					{
						queueEntry.Priority = filteredList[filteredList.Count - 1].Priority + 1000;
					}
					else
					{
						queueEntry.Priority = (filteredList[requestedIndex - 1].Priority + filteredList[requestedIndex].Priority) * 0.5f;
					}
					return true;
				}
			}
			return false;
		}

		public bool RemoveQueuePriority(Guid listingId, Guid queueEntryId)
		{
			if (QueueEntries.TryGetValue(listingId, out var listingQueues))
			{
				ListingQueueEntryResponseDto? queueEntry = listingQueues.FirstOrDefault(q => q.Id == queueEntryId);
				if (queueEntry != null)
				{
					queueEntry.Priority = null;
					return true;
				}
			}
			return false;
		}
	}
}
