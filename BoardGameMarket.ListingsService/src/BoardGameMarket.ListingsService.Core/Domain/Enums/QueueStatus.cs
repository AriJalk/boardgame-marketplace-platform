namespace BoardGameMarket.ListingsService.Core.Domain.Enums
{
	public enum QueueStatus
	{
		Pending,    // waiting buyer/seller action
		Approved,   // transaction approved but not finalized
		Completed,  // transaction finished
		Cancelled
	}
}
