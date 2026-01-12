using TestingService.Domains.Queues.Enums;
using TestingService.Domains.Users.Dtos;

namespace TestingService.Domains.Queues.Dtos
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
