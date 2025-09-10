using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class PriorityQueueDomainService
	{
		public void ValidatePriorityEntry(ListingQueueEntry listingQueueEntry, IEnumerable<ListingQueuePriorityEntry> listingQueuePriorityEntries, decimal priorityWeight, bool shouldExist)
		{
			List<string> errors = new List<string>();
			bool exists = listingQueuePriorityEntries.Any(qpe => qpe.ListingQueueEntryId == listingQueueEntry.Id);
			// Validate existance
			if (shouldExist && !exists)
			{
				errors.Add("Queue entry not exists in priority queue");
			}
			else if(!shouldExist && exists)
			{
				errors.Add("Queue entry already exists in priority queue");
			}

			// Validate priority uniqueness
			if (listingQueuePriorityEntries.Any(qpe => qpe.Priority == priorityWeight))
			{
				errors.Add("Entry priority weight not unique in priority queue");
			}
			if (errors.Count > 0)
			{
				throw new InvalidOperationException(string.Join("\n", errors));
			}
		}
	}
}
