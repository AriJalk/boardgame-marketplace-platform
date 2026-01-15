using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	public class ListingQueueEntryResponseDto
	{
		public Guid Id { get; set; }
		public QueueStatus Status { get; set; }
		public UserSummaryDto? Buyer { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public decimal? Priority { get; set; }
	}
}
