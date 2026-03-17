using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
    public class UsersDataService
    {
        private const string CACHE_KEY = "user:{userId}";

        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;

        public UsersDataService(UsersHttpClient httpClient, IDistributedCache distributedCache)
        {
            _httpClient = httpClient.HttpClient;
            _distributedCache = distributedCache;
        }

        private async Task<UserInfo?> TryGetCachedUser(Guid userId)
        {

            return await CacheHelper.GetCached<UserInfo>(_distributedCache, CACHE_KEY);
        }

        public async Task<UserInfo?> GetUser(Guid userId)
        {
            UserInfo? cachedUser = await TryGetCachedUser(userId);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            string uri = $"api/users/{userId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            UserInfo? userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

            if (userInfo != null)
            {
                await CacheHelper.WriteToCache(_distributedCache, CACHE_KEY, userInfo);
            }

            return userInfo;
        }

        public async Task<UserInfo> GetUserOrThrowAsync(Guid userId)
        {
            string uri = $"api/users/{userId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException();
            }

            response.EnsureSuccessStatusCode();

            UserInfo? userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

            if (userInfo == null)
            {
                throw new InvalidDataException();
            }
            return userInfo;
        }

        public async Task<UserInfo> CreateUser(CreateUserRequest createUserRequest)
        {
            string uri = $"api/users";

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, createUserRequest);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new InvalidOperationException("User exists");
            }

            UserInfo? userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

            return userInfo ?? throw new InvalidDataException();
        }

        public async Task<string> GetEmail(Guid userId)
        {
            string uri = $"api/users/{userId}/contact-email";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            string? email = await response.Content.ReadAsStringAsync();

            if (email == null)
            {
                throw new InvalidDataException();
            }

            return email;
        }
    }
}
