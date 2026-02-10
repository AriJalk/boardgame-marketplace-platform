using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos
{
	/// <summary>
	/// Buyer perspective of queue entry
	/// </summary>
	/// <param name="Id"></param>
	/// <param name="Status"></param>
	/// <param name="CreatedAt"></param>
	/// <param name="ModifiedAt"></param>
	public record ListingSubscriptionResponseDto(Guid Id, QueueStatus Status, DateTime CreatedAt, DateTime ModifiedAt)
	{
	}
}
