namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services
{
	public class CatalogHttpClient
	{
		public HttpClient HttpClient { get; set; }
		public CatalogHttpClient(HttpClient httpClient) 
		{
			HttpClient = httpClient;
		}
	}
}
