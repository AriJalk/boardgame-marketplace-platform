using TestingService.Core.Domain.Enums;
using TestingService.Dtos;

namespace TestingService.Core.Application.DTOs.Responses
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
