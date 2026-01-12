namespace TestingService.Domains.Listings.Enums
{
	public enum QueueStatus
	{
		Queued,
		Approved,   // transaction approved but not finalized
		Completed,  // transaction finished
		Cancelled
	}
}
