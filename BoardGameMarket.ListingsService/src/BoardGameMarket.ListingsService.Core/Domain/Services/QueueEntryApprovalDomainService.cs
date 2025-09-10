using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class QueueEntryApprovalDomainService
	{
		private bool IsQueueEntryValid(ListingQueueEntry listingQueueEntry)
		{
			return listingQueueEntry.Status == QueueStatus.Queued;
		}

		private bool IsListingValid(Listing listing)
		{
			return listing.Status == ListingStatus.Active;
		}

		private bool IsPrioritySaleValid(ListingQueueEntry listingQueueEntry, ListingQueuePriorityEntry topPriorityEntry)
		{
			return listingQueueEntry.Id == topPriorityEntry.ListingQueueEntryId;
		}
		private bool IsRegularSaleValid(ListingQueueEntry listingQueueEntry, ListingQueueEntry topListingQueueEntry)
		{
			return listingQueueEntry.Id == topListingQueueEntry.Id;
		}

		public Result CanApprove(ListingQueueEntry requestQueueEntry, Listing listing, Queue<ListingQueueEntry> listingQueueEntries, Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries)
		{
			if (!IsListingValid(listing))
			{
				return Result.Failure("Listing is not in active status");
			}
			if (!IsQueueEntryValid(requestQueueEntry))
			{
				return Result.Failure("Queue entry is not in queued status");
			}

			// Priority queue first
			if (listingQueuePriorityEntries.TryPeek(out ListingQueuePriorityEntry? headPriorityEntry))
			{
				return IsPrioritySaleValid(requestQueueEntry, headPriorityEntry)
					? Result.Success()
					: Result.Failure("QueueEntry not matching Head of priority list");
			}
			else if (listingQueueEntries.TryPeek(out ListingQueueEntry? headEntry))
			{
				return IsRegularSaleValid(requestQueueEntry, headEntry)
					? Result.Success()
					: Result.Failure("QueueEntry not matching head of queue");

			}

			return Result.Failure("Queue is empty");
		}
	}
}
