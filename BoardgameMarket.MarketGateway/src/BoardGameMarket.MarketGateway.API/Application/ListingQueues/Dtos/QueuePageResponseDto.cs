namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> QueueByTime, IEnumerable<ListingQueueEntryResponseDto> QueueByPriority)
	{

	}
}
