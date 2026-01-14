
namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Services
{
	public class ListingsHttpClient
	{
		public HttpClient HttpClient { get; init; }

		public ListingsHttpClient(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

	}
}
