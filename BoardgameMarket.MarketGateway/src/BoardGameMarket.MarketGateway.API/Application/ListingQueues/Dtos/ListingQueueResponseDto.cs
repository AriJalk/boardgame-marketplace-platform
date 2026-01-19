namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	public record ListingQueueResponseDto(ListingQueueEntryResponseDto[] QueueByTime, ListingQueueEntryResponseDto[] QueueByPriority)
	{

	}
}
