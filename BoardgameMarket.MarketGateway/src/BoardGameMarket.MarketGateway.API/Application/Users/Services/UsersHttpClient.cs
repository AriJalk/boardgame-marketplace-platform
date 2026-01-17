namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
	public class UsersHttpClient
	{
		public HttpClient HttpClient { get; init; }

		public UsersHttpClient(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}
	}
}
