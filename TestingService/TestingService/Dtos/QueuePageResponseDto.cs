using TestingService.Core.Application.DTOs.Responses;

namespace TestingService.Dtos
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> TimeQueue, IEnumerable<ListingQueueEntryResponseDto> PriorityQueue)
	{

	}
}
