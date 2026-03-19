using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
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
		private RedisCacheService _cacheService;

		public CatalogDataService(CatalogHttpClient catalogHttpClient, RedisCacheService redisCacheService)
		{
			_httpClient = catalogHttpClient.HttpClient;
			_cacheService = redisCacheService;
		}

		public async Task<CatalogItemInfo> GetCatalogItemByInternalId(Guid catalogItemId)
		{
			string cacheKey = $"catalog-item:{catalogItemId}";
			CatalogItemInfo? cachedItem = await _cacheService.GetCachedAsync<CatalogItemInfo>(cacheKey);

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

			await _cacheService.WriteToCacheAsync(cacheKey, catalogItemInfo, TimeSpan.FromMinutes(5));

			return catalogItemInfo;
		}

		public async Task<CatalogItemInfo> GetBggCatalogItemByBggId(int bggId)
		{
            string cacheKey = $"bgg-id:{bggId}";
            CatalogItemInfo? cachedItem = await _cacheService.GetCachedAsync<CatalogItemInfo>(cacheKey);

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

			List<string> names = request.Names.Distinct().ToList();

			var (exists, missing) = await _cacheService.GetCachedAsync<CatalogItemInfo>(cacheKey, names);
			if (!missing.Any())
			{
				return exists.ToArray();
			}

			string uri = $"api/catalog/custom";
			CustomCatalogItemsRequest missingRequest = new CustomCatalogItemsRequest(missing.ToArray());
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, missingRequest);

			response.EnsureSuccessStatusCode();

			IEnumerable<CatalogItemInfo>? missingItems = await response.Content.ReadFromJsonAsync<IEnumerable<CatalogItemInfo>>();

			if (missingItems == null)
			{
				throw new InvalidDataException();
            }

			Dictionary<string, CatalogItemInfo> missingDict = missingItems.ToDictionary(i => i.Name);

			await _cacheService.WriteToCacheAsync(cacheKey, missingDict, TimeSpan.FromMinutes(5));

			return exists.Concat(missingItems).ToArray();
		}
	}
}
