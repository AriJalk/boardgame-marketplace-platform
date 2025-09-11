using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries
{
	internal class ListingQueueEntryAddRequest
	{
		public Guid ListingId { get; set; }
		public QueueStatus Status { get; set; }
		public Guid BuyerUserId { get; set; }
	}
}
