namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries
{
	internal interface IListingQueuePriorityEntryDeleterService
	{
		Task<bool> DeleteListingQueuePriorityEntry(Guid listingQueueEntryId);
	}
}
