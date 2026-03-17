using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services
{
	public class CatalogDataService
	{
		private HttpClient _httpClient;
		private IDistributedCache _distributedCache;

		public CatalogDataService(CatalogHttpClient catalogHttpClient, IDistributedCache distributedCache)
		{
			_httpClient = catalogHttpClient.HttpClient;
			_distributedCache = distributedCache;
		}

		public async Task<CatalogItemInfo> GetCatalogItemByInternalId(Guid catalogItemId)
		{
			string cacheKey = $"item:{catalogItemId}";
			CatalogItemInfo? cachedItem = await CacheHelper.GetCached<CatalogItemInfo>(_distributedCache, cacheKey);

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

		public async Task<CatalogItemInfo[]> GetCustomCatalogItemsByNames(CustomCatalogItemsRequest request)
		{
			string cacheKey = $"catalog-item";

			string uri = $"api/catalog/custom";
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, request);

			response.EnsureSuccessStatusCode();

			CatalogItemInfo[]? items = await response.Content.ReadFromJsonAsync<CatalogItemInfo[]>();

			return items ?? throw new InvalidDataException();
		}
	}
}
