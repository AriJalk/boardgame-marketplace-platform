using AutoFixture;
using System.Reflection.Metadata.Ecma335;
using TestingService.Core.Application.DTOs.Responses;
using TestingService.Dtos;

namespace TestingService.Services
{
	public class ListingsService
	{
		Guid USER_1 = Guid.Parse("4552ea90-84c8-4d7c-8409-d80b39cad771");
		Guid USER_2 = Guid.Parse("4552ea90-84c8-4d7c-8409-d80b39cad772");

		Guid LISTING_GROUP_1 = Guid.Parse("5559E221-B571-48B0-9A59-77EC841AC376");
		Guid LISTING_GROUP_2 = Guid.Parse("E5052EF1-0002-462A-8ECB-79558A63CBAD");

		Guid LISTING_1 = Guid.Parse("DDDEEEDB-F24D-499A-AD55-B7F8877CB659");
		Guid LISTING_2 = Guid.Parse("A6037400-9A54-4AF3-9039-F0ADA9A1D0CC");

		Guid LISTING_3 = Guid.Parse("2136A518-7DFC-4162-B5F0-21C870785437");
		Guid LISTING_4 = Guid.Parse("469996FD-E1A4-4683-8DF4-EABAA107D4D3");

		Fixture fixture = new Fixture();

		public Dictionary<Guid, ListingGroupDetailedDto> ListingGroups = new Dictionary<Guid, ListingGroupDetailedDto>();
		public Dictionary<Guid, ListingDetailDto> Listings = new Dictionary<Guid, ListingDetailDto>();
		public Dictionary<Guid, List<ListingQueueEntryResponseDto>> QueueEntries = new Dictionary<Guid, List<ListingQueueEntryResponseDto>>();
		public Dictionary<Guid, UserSummaryDto> Users = new Dictionary<Guid, UserSummaryDto>();

		public ListingsService()
		{
			UserSummaryDto user1 = new UserSummaryDto(USER_1, "User-1");
			UserSummaryDto user2 = new UserSummaryDto(USER_2, "User-2");

			ListingDetailDto listing1 = new ListingDetailDto(LISTING_1, "listing-1", 200, "lorem ipsum", fixture.Create<IEnumerable<ListingItemDto>>());
			ListingDetailDto listing2 = new ListingDetailDto(LISTING_2, "listing-2", 300, "lorem ipsum", fixture.Create<IEnumerable<ListingItemDto>>());
			ListingDetailDto listing3 = new ListingDetailDto(LISTING_3, "listing-3", 400, "lorem ipsum", fixture.Create<IEnumerable<ListingItemDto>>());
			ListingDetailDto listing4 = new ListingDetailDto(LISTING_4, "listing-4", 500, "lorem ipsum", fixture.Create<IEnumerable<ListingItemDto>>());

			ListingGroupDetailedDto listingGroup1 = new ListingGroupDetailedDto(LISTING_GROUP_1, user1, DateTime.UtcNow, "pickup", "Rehovot", "lorem ipsum", new List<ListingDetailDto>() { listing1, listing2 });
			ListingGroupDetailedDto listingGroup2 = new ListingGroupDetailedDto(LISTING_GROUP_2, user1, DateTime.UtcNow, "delivery/pickup", "Rishon", "lorem ipsum", new List<ListingDetailDto>() { listing3, listing4 });

			Users.Add(user1.Id, user1);
			Users.Add(user2.Id, user2);

			Listings.Add(listing1.Id, listing1);
			Listings.Add(listing2.Id, listing2);
			Listings.Add(listing3.Id, listing3);
			Listings.Add(listing4.Id, listing4);

			ListingGroups.Add(listingGroup1.Id, listingGroup1);
			ListingGroups.Add(listingGroup2.Id, listingGroup2);

			int gap = 0;
			QueueEntries.Add(LISTING_1, fixture.Build<ListingQueueEntryResponseDto>().With(q => q.Priority, null as float?).CreateMany(10).ToList());
			foreach (var entry in QueueEntries[LISTING_1].Skip(5))
			{
				entry.Priority = gap * 1000f;
				gap++;
			}
			gap = 0;
			QueueEntries.Add(LISTING_2, fixture.Build<ListingQueueEntryResponseDto>().With(q => q.Priority, null as float?).CreateMany(10).ToList());
			foreach (var entry in QueueEntries[LISTING_2].Skip(6))
			{
				entry.Priority = gap * 1000f;
				gap++;
			}
			gap = 0;
			QueueEntries.Add(LISTING_3, fixture.Build<ListingQueueEntryResponseDto>().With(q => q.Priority, null as float?).CreateMany(10).ToList());
			foreach (var entry in QueueEntries[LISTING_3].Skip(4))
			{
				entry.Priority = gap * 1000f;
				gap++;
			}
			gap = 0;
			QueueEntries.Add(LISTING_4, fixture.Build<ListingQueueEntryResponseDto>().With(q => q.Priority, null as float?).CreateMany(10).ToList());
			foreach (var entry in QueueEntries[LISTING_4].Skip(8))
			{
				entry.Priority = gap * 1000f;
				gap++;
			}
		}

		public bool UpdateQueuePriority(Guid listingId, Guid queueEntryId, int requestedIndex)
		{
			if (QueueEntries.TryGetValue(listingId, out var listingQueues))
			{
				ListingQueueEntryResponseDto? queueEntry = listingQueues.FirstOrDefault(q => q.Id == queueEntryId);
				if (queueEntry != null)
				{
					var orderedList = listingQueues.Where(q => q.Priority != null).OrderBy(q => q.Priority).ToList();
					if (requestedIndex == 0)
					{
						queueEntry.Priority = orderedList[0].Priority - 1000;
					}
					else if (requestedIndex >= orderedList.Count - 1)
					{
						queueEntry.Priority = orderedList[orderedList.Count - 1].Priority + 1000;
					}
					else
					{
						var filtered = orderedList.Where(q => q.Id != queueEntry.Id).ToList();
						queueEntry.Priority = (filtered[requestedIndex - 1].Priority + filtered[requestedIndex].Priority) * 0.5f;
					}
					return true;
				}
			}
			return false;
		}

	}
}
