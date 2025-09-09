using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Responses
{
	public class ListingQueueEntryResponseDto
	{
		public Guid Id { get; set; }
		public QueueStatus Status { get; set; }
		public Guid BuyerUserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }

		public decimal? PriorityWeight { get; set; }
	}
}
