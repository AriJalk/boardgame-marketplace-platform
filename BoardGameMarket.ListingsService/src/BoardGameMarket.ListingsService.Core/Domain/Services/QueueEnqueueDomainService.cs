using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class QueueEnqueueDomainService
	{
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
	}
}
