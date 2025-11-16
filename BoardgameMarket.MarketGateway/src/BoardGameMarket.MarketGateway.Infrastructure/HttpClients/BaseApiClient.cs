using BoardGameMarket.MarketGateway.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Json;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.Infrastructure.HttpClients
{
	internal abstract class BaseApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IDistributedCache _cache;
		private readonly ICacheOptionsProvider _cacheOptionsProvider;

		protected BaseApiClient(HttpClient httpClient, IDistributedCache cache, ICacheOptionsProvider cacheOptionsProvider)
		{
			_httpClient = httpClient;
			_cache = cache;
			_cacheOptionsProvider = cacheOptionsProvider;
		}

		protected async Task<T?> GetFromApiAsync<T>(string endpoint, string cacheKey) where T : class
		{
			try
			{
				string? cachedEntity = await _cache.GetStringAsync(cacheKey);
				if (cachedEntity != null)
				{
					T? entityFromCache = JsonSerializer.Deserialize<T>(cachedEntity);

					return entityFromCache;
				}
				HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
				if (!response.IsSuccessStatusCode)
				{
					return null;
				}
				T? entity = await response.Content.ReadFromJsonAsync<T?>();
				if (entity == null)
				{
					throw new ArgumentException("Invalid entity");
				}

				string entityJson = JsonSerializer.Serialize(entity);

				await _cache.SetStringAsync(cacheKey, entityJson, _cacheOptionsProvider.Default);

				return entity;
			}
			catch
			{
				throw;
			}
		}

		protected async Task<T?> PostToApiAsync<T>(string endpoint, T postEntity) where T : class
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsJsonAsync<T>(endpoint, postEntity);
				if (!response.IsSuccessStatusCode)
				{
					return null;
				}

				T? responseEntity = await response.Content.ReadFromJsonAsync<T?>();
				if (responseEntity == null)
				{
					throw new ArgumentException("Invalid entity");
				}
				return responseEntity;
			}
			catch
			{
				throw;
			}
		}
	}
}