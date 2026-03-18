using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.Application.Shared
{
    public class RedisCacheService
    {
        private readonly IDatabase _redisDb;
        private readonly IDistributedCache _stackExchangeCache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, IDistributedCache stackExchangeCache, ILogger<RedisCacheService> logger)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
            _stackExchangeCache = stackExchangeCache;
            _logger = logger;
        }
        public async Task<T?> GetCachedAsync<T>(string key)
        {

            string? cachedResult = await _stackExchangeCache.GetStringAsync(key);
            if (cachedResult == null)
            {
                return default;
            }

            try
            {
                T? result = JsonSerializer.Deserialize<T>(cachedResult);
                return result;
            }
            catch
            {
                _logger.LogDebug("Can't deserialize");
                return default;
            }
        }

        public async Task<(IEnumerable<T>, IEnumerable<string>)> GetCachedAsync<T>(string key, IEnumerable<string> keyValues)
        {

            var results = await Task.WhenAll(keyValues.Select(async value => 
            {
                T? item = await GetCachedAsync<T>(string.Concat(key, ":", value));
                return (value, item);
            }));

            var exists = results.Where(r => r.item != null).Select(r => r.item!).ToList();
            var missing = results.Where(r => r.item == null).Select(r => r.value).ToList();

            return (exists, missing);
        }


        public async Task WriteToCacheAsync(string key, object value, TimeSpan? expiration = null)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            string valueJson = JsonSerializer.Serialize(value);

            await _stackExchangeCache.SetStringAsync(key, valueJson, options);
        }

        public async Task WriteToCacheAsync<T>(string key, Dictionary<string, T> values, TimeSpan? expiration = null)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            var tasks = values.Select(async kv =>
            {
                string valueJson = JsonSerializer.Serialize(kv.Value);
                await _stackExchangeCache.SetStringAsync(string.Concat(key, ":", kv.Key), valueJson, options);
            });

            await Task.WhenAll(tasks);
        }



        public async Task<(bool Acquired, string? ExistingValue)> TryStartIdempotentAsync(string key)
        {
            bool acquired = await _redisDb.StringSetAsync(
                key,
                "processing",
                TimeSpan.FromSeconds(60),
                When.NotExists
            );

            if (acquired)
                return (true, null);

            string? existing = await _redisDb.StringGetAsync(key);
            return (false, existing);
        }

        public async Task CompleteIdempotentAsync(string key, Guid result)
        {
            await _redisDb.StringSetAsync(
                key,
                result.ToString(),
                TimeSpan.FromMinutes(5)
            );
        }
    }
}
