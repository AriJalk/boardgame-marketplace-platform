using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	public record ListingQueueEntryResponseDto(Guid Id, QueueStatus Status, UserSummaryDto Buyer, DateTime CreatedAt, DateTime ModifiedAt, decimal? Priority)
	{
	}
}
