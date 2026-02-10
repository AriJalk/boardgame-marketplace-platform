using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	/// <summary>
	/// Seller perspective of queue entry
	/// </summary>
	/// <param name="Id"></param>
	/// <param name="Status"></param>
	/// <param name="Buyer"></param>
	/// <param name="CreatedAt"></param>
	/// <param name="ModifiedAt"></param>
	/// <param name="Priority"></param>
	public record ListingQueueEntryResponseDto(Guid Id, QueueStatus Status, UserSummaryDto? Buyer, DateTime CreatedAt, DateTime ModifiedAt, decimal? Priority)
	{
	}
}
