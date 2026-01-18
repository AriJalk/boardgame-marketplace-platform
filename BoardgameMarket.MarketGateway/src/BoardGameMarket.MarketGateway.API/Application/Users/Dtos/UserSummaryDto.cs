namespace BoardGameMarket.MarketGateway.API.Application.Users.Dtos
{
	public class UserSummaryDto
	{
		public Guid UserId { get; set; }
		public string? DisplayName { get; set; }
	}
}
