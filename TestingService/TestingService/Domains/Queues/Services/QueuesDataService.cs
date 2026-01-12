using TestingService.Domains.Listings.ServiceClients.Listings;
using TestingService.Domains.Listings.Services;
using TestingService.Domains.Queues.Dtos;
using TestingService.Domains.Queues.ServiceClients;
using TestingService.Domains.Users.ServiceClients;
using TestingService.Domains.Users.Services;

namespace TestingService.Domains.Queues.Services
{
	public class QueuesDataService
	{
		private const int PRIORITY_GAP = 1000;
		private ListingsDataService _listingService;
		private UsersService _usersService;
		private Random _random = new Random();

		public Dictionary<Guid, List<ListingQueueEntryInfo>> ListingQueueEntries = new Dictionary<Guid, List<ListingQueueEntryInfo>>();



		public QueuesDataService(ListingsDataService listingsService, UsersService usersService)
		{
			_listingService = listingsService;
			_usersService = usersService;
			foreach (ListingDetail listing in listingsService.Listings.Values)
			{
				SeedListingQueueMock(listing);
			}
		}

		private void SeedListingQueueMock(ListingDetail listing)
		{
			int gap = 0;
			int userCount = _usersService.Users.Count;
			List<UserInfo> subscribedUsers = new List<UserInfo>();
			List<ListingQueueEntryInfo> queueEntries = new List<ListingQueueEntryInfo>();
			for (int i = 0; i < 10; i++)
			{
				UserInfo? user = null;
				do
				{
					int index = _random.Next(userCount - 1);
					user = _usersService.Users.Values.ElementAt(index);
				}
				while(subscribedUsers.Contains(user));
				subscribedUsers.Add(user);

				ListingQueueEntryInfo queueEntry = new ListingQueueEntryInfo()
				{
					Id = Guid.NewGuid(),
					ListingId = listing.Id,
					BuyerUserId = user.Id,
					CreatedAt = DateTime.Now.AddDays(-gap),
					ModifiedAt = DateTime.Now,
					Status = Enums.QueueStatus.Queued,
					Priority = i % 2 == 0 ? gap++ * PRIORITY_GAP : null,
				};
				queueEntries.Add(queueEntry);
			}
			ListingQueueEntries[listing.Id] = queueEntries;
		}

		public bool UpdateQueuePriority(Guid listingId, Guid queueEntryId, int requestedIndex)
		{
			if (ListingQueueEntries.TryGetValue(listingId, out var listingQueues))
			{
				ListingQueueEntryInfo? queueEntry = listingQueues.FirstOrDefault(q => q.Id == queueEntryId);
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
						queueEntry.Priority = (filteredList[requestedIndex - 1].Priority + filteredList[requestedIndex].Priority) * 0.5m;
					}
					return true;
				}
			}
			return false;
		}

		public bool RemoveQueuePriority(Guid listingId, Guid queueEntryId)
		{
			if (ListingQueueEntries.TryGetValue(listingId, out var listingQueues))
			{
				ListingQueueEntryInfo? queueEntry = listingQueues.FirstOrDefault(q => q.Id == queueEntryId);
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
