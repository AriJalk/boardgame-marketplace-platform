namespace TestingService.Dtos.Queue
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> TimeQueue, IEnumerable<ListingQueueEntryResponseDto> PriorityQueue)
	{

	}
}
