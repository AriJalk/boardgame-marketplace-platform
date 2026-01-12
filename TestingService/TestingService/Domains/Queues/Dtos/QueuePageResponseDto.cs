namespace TestingService.Domains.Queues.Dtos
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> TimeQueue, IEnumerable<ListingQueueEntryResponseDto> PriorityQueue)
	{

	}
}
