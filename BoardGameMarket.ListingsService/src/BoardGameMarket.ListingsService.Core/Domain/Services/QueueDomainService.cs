using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	public class QueueDomainService
	{
		private Result ValidateHeadEntry(IEnumerable<ListingQueueEntry> queueEntries, QueueStatus requiredStatus, out ListingQueueEntry? headEntry)
		{
			List<string> errors = new List<string>();
			Queue<ListingQueueEntry> queue = new Queue<ListingQueueEntry>(SortQueue(queueEntries));
			if (!queue.TryDequeue(out headEntry))
			{
				errors.Add("Empty queue");
				return Result.Failure(errors);
			}
			if (headEntry.Status != requiredStatus)
			{
				errors.Add($"Head entry not in {requiredStatus} status");
			}
			if (errors.Count != 0)
			{
				return Result.Failure(errors);
			}
			return Result.Success();
		}
		public Result CanEnqueue(Guid sellerUserId, Guid buyerUserId, IEnumerable<ListingQueueEntry> queueEntries)
		{
			List<string> errors = new List<string>();
			if (buyerUserId == sellerUserId)
			{
				errors.Add("BuyerUserId can't be same as SellerUserId");
			}
			if (queueEntries.Any(e => e.BuyerUserId == buyerUserId))
			{
				errors.Add("Buyer can't enqueue more than once for a listing");
			}
			if (errors.Count != 0)
			{
				return Result.Failure(errors);
			}
			return Result.Success();
		}

		public Result CanApproveHead(IEnumerable<ListingQueueEntry> queueEntries, out ListingQueueEntry? headEntry)
		{
			return ValidateHeadEntry(queueEntries, QueueStatus.Queued, out headEntry);
		}

		public Result CanSellHead(IEnumerable<ListingQueueEntry> queueEntries, out ListingQueueEntry? headEntry)
		{
			return ValidateHeadEntry(queueEntries, QueueStatus.Approved, out headEntry);
		}

		public IEnumerable<ListingQueueEntry> SortQueue(IEnumerable<ListingQueueEntry> listingQueueEntries)
		{
			return listingQueueEntries.OrderByDescending(qe => qe.Priority).ThenBy(qe => qe.CreatedAt);
		}
	}
}
