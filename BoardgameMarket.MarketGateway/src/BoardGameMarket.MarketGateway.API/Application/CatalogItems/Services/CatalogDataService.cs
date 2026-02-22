using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services
{
	public class CatalogDataService
	{
		private HttpClient _httpClient;
		public CatalogDataService(CatalogHttpClient catalogHttpClient)
		{
			_httpClient = catalogHttpClient.HttpClient;
		}

		public async Task<CatalogItemInfo> GetCatalogItemByInternalId(Guid catalogItemId)
		{
			string uri = $"api/catalog/{catalogItemId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new InvalidOperationException();
			}

			response.EnsureSuccessStatusCode();

			CatalogItemInfo? catalogItemInfo = await response.Content.ReadFromJsonAsync<CatalogItemInfo>();

			if (catalogItemInfo == null)
			{
				throw new InvalidDataException();
			}
			return catalogItemInfo;
		}

		public async Task<CatalogItemInfo> GetBggCatalogItemByBggId(int bggId)
		{
			string uri = $"api/catalog/bgg/{bggId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new KeyNotFoundException();
			}

			response.EnsureSuccessStatusCode();

			CatalogItemInfo? catalogItemInfo = await response.Content.ReadFromJsonAsync<CatalogItemInfo>();

			if (catalogItemInfo == null)
			{
				throw new InvalidDataException();
			}
			return catalogItemInfo;
		}
	}
}
