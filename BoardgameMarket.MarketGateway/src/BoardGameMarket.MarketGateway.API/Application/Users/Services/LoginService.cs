using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
	public class LoginService
	{
		public UserInfo? LoggedInUser { get; set; }

		public LoginService() { }
	}
}
