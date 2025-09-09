using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.ServiceContracts;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class QueueDomainService : IQueueDomainService
	{
		public Result CanEnqueue(Listing listing, ListingGroup listingGroup, Guid buyerUserId, IEnumerable<ListingQueueEntry> queueEntries)
		{
			List<string> errors = new List<string>();
			if (buyerUserId == listingGroup.SellerUserId)
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
