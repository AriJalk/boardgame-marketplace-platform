using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers
{
	public static class ListingQueueEntryMappers
	{
		public static ListingQueueEntryResponseDto ToListingQueueEntryResponseDto(this ListingQueueEntryInfo queueEntry, UserSummaryDto? userSummaryDto)
		{
			return new ListingQueueEntryResponseDto(
				queueEntry.Id,
				queueEntry.Status,
				userSummaryDto,
				queueEntry.CreatedAt,
				queueEntry.ModifiedAt,
				queueEntry.Priority);
		}

		public static ListingSubscriptionResponseDto ToListingSubscriptionResponseDto(this ListingQueueEntryInfo queueEntry)
		{
			return new ListingSubscriptionResponseDto(
				queueEntry.Id, 
				queueEntry.Status, 
				queueEntry.CreatedAt, 
				queueEntry.ModifiedAt);
		}
	}
}
