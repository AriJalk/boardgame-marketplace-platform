using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters
{
	public class ListingQueueEntryFilter
	{
		public Guid? UserId { get; init; }
		public Guid[]? ListingIds { get; init; }
        public IReadOnlyCollection<Guid>? ListingQueueEntryIds { get; init; }
        public ListingStatus? ListingStatus { get; init; }
	}
}
