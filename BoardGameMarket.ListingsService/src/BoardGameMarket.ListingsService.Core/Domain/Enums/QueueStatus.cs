namespace BoardGameMarket.ListingsService.Core.Domain.Enums
{
	public enum QueueStatus
	{
		Queued,
		Pending,    // waiting buyer/seller action
		Approved,   // transaction approved but not finalized
		Completed,  // transaction finished
		Cancelled
	}
}
