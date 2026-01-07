using System.Reflection;
using System.Text.Json;
using TestingService.Core.Application.DTOs.Responses;
using TestingService.Dtos;

namespace TestingService.Services
{

	public class ListingsService
	{

		public Dictionary<Guid, ListingGroupDetailedDto> ListingGroups = new Dictionary<Guid, ListingGroupDetailedDto>();
		public Dictionary<Guid, ListingDetailDto> Listings = new Dictionary<Guid, ListingDetailDto>();
		public Dictionary<Guid, List<ListingQueueEntryResponseDto>> QueueEntries = new Dictionary<Guid, List<ListingQueueEntryResponseDto>>();
		public Dictionary<Guid, UserSummaryDto> Users = new Dictionary<Guid, UserSummaryDto>();

		public ListingsService(IWebHostEnvironment hostEnvironment)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "MOCK_DATA.json"));

			IEnumerable<ListingGroupDetailedDto>? listingGroups = JsonSerializer.Deserialize<IEnumerable<ListingGroupDetailedDto>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

			if (listingGroups == null)
				return;
			foreach (ListingGroupDetailedDto listingGroup in listingGroups)
			{
				BuildListingGroup(listingGroup);
			}
		}

		private void BuildListingGroup(ListingGroupDetailedDto listingGroup)
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
