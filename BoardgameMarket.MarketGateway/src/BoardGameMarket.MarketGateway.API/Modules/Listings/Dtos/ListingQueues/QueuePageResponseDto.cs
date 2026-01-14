namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingQueues
{
	public record QueuePageResponseDto(IEnumerable<ListingQueueEntryResponseDto> QueueByTime, IEnumerable<ListingQueueEntryResponseDto> QueueByPriority)
	{

	}
}
