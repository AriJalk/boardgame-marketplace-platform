namespace BoardGameMarket.MarketGateway.API.Modules.Queues.Enums
{
	public enum QueueStatus
	{
		Queued,
		Approved,   // transaction approved but not finalized
		Completed,  // transaction finished
		Cancelled
	}
}
