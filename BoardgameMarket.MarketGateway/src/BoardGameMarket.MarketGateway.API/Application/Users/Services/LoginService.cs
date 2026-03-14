using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
	public class LoginService
	{
		public UserInfo? LoggedInUser { get; set; }

		public LoginService() { }

		public Guid GetUserIdOrThrow()
		{
			if (LoggedInUser == null)
			{
				throw new InvalidOperationException("User not logged in");
			}

			return LoggedInUser.Id;
		}
	}
}
