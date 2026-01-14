using BoardGameMarket.MarketGateway.API.Modules.Queues.Enums;
using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingQueues
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
