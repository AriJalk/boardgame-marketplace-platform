namespace BoardGameMarket.MarketGateway.API.Modules.Queues.Dtos
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> TimeQueue, IEnumerable<ListingQueueEntryResponseDto> PriorityQueue)
	{

	}
}
