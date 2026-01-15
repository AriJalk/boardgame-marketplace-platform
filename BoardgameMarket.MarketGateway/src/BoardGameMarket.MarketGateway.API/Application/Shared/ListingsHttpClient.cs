namespace BoardGameMarket.MarketGateway.API.Application.Shared
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
