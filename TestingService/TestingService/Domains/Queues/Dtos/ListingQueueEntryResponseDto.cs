using TestingService.Core.Domain.Enums;
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
		public float? Priority { get; set; }
	}
}
