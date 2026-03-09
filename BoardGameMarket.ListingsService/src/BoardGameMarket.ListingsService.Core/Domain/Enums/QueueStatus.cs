namespace BoardGameMarket.ListingsService.Core.Domain.Enums
{
	public enum QueueStatus
	{
		Queued,
		Reserved,   // transaction approved but not finalized
		Cancelled
	}
}
