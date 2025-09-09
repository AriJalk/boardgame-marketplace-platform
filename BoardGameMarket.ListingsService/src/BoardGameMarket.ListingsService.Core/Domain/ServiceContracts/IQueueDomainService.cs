using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.ServiceContracts
{
	internal interface IQueueDomainService
	{
		Result CanEnqueue(Listing listing, ListingGroup listingGroup, Guid buyerUserId, IEnumerable<ListingQueueEntry> queueEntries);
	}
}
