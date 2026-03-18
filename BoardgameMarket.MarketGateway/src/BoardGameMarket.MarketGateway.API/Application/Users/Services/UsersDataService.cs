using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
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
        private readonly HttpClient _httpClient;
        private readonly RedisCacheService _cacheService;

        public UsersDataService(UsersHttpClient httpClient, RedisCacheService redisCacheService)
        {
            _httpClient = httpClient.HttpClient;
            _cacheService = redisCacheService;
        }

        public async Task<UserInfo?> GetUser(Guid userId)
        {
            string cacheKey = $"user:{userId}";
            UserInfo? cachedUser = await _cacheService.GetCachedAsync<UserInfo>(cacheKey);
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
                await _cacheService.WriteToCacheAsync(cacheKey, userInfo, TimeSpan.FromMinutes(5));
            }

            return userInfo;
        }

        public async Task<UserInfo> GetUserOrThrowAsync(Guid userId)
        {
            UserInfo? userInfo = await GetUser(userId);

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

            if (userInfo == null)
            {
                throw new InvalidDataException();
            }

            string cacheKey = $"user:{userInfo.Id}";
            await _cacheService.WriteToCacheAsync(cacheKey, userInfo, TimeSpan.FromMinutes(5));

            return userInfo;
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
