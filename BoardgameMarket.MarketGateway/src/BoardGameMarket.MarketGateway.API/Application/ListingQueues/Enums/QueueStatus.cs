namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums
{
	public enum QueueStatus
	{
		Queued,
		Approved,   // transaction approved but not finalized
		Completed,  // transaction finished
		Cancelled
	}
}
