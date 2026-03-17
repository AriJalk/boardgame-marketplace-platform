using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.Helpers
{
    public static class CacheHelper
    {
        public static async Task<T?> GetCached<T>(IDistributedCache cache, string key)
        {

            string? cachedResult = await cache.GetStringAsync(key);
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
                return default;
            }
        }

        public static async Task WriteToCache(this IDistributedCache cache, string key, object value, TimeSpan? expiration = null)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            string valueJson = JsonSerializer.Serialize(value);

            await cache.SetStringAsync(key, valueJson, options);
        }

        public static async Task<ValueTuple<IEnumerable<T>, IEnumerable<string>>> GetCached<T>(IDistributedCache cache, string key, IEnumerable<string> keyValues)
        {
            List<T> exists = new List<T>(); ;
            List<string> missingKeys = new List<string>();

            foreach (string value in keyValues)
            {
                T? item = await GetCached<T>(cache, key+":"+value);
                if (item != null)
                {
                    exists.Add(item);
                }
                else
                {
                    missingKeys.Add(value);
                }
            }
            return (exists, missingKeys);
        }
    }
}
