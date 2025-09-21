using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingQueueEntryMappers
	{
		public static ListingQueueEntryResponseDto ToDto(this ListingQueueEntry listingQueueEntry)
		{
			return new ListingQueueEntryResponseDto()
			{
				Id = listingQueueEntry.Id,
				BuyerUserId = listingQueueEntry.BuyerUserId,
				CreatedAt = listingQueueEntry.CreatedAt,
				Status = listingQueueEntry.Status,
				Priority = listingQueueEntry.Priority
			};
		}

		public static ListingQueueEntry ToListingQueueEntry(this ListingQueueEntryAddRequest listingQueueEntryAddRequest)
		{
			return new ListingQueueEntry()
			{
				ListingId = listingQueueEntryAddRequest.ListingId,
				BuyerUserId = listingQueueEntryAddRequest.BuyerUserId,
				Status = listingQueueEntryAddRequest.Status,
				Priority = listingQueueEntryAddRequest.Priority
			};
		}

		public static ListingQueueEntryUpdateRequest ToUpdateRequest(this ListingQueueEntry listingQueueEntry)
		{
			return new ListingQueueEntryUpdateRequest()
			{
				Id = listingQueueEntry.Id,
				ListingId = listingQueueEntry.ListingId,
				BuyerUserId = listingQueueEntry.BuyerUserId,
				Status = listingQueueEntry.Status,
				Priority = listingQueueEntry.Priority
			};
		}
	}
}
