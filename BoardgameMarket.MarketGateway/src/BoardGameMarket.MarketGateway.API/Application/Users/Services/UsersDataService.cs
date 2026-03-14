using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
	public class UsersDataService
	{
		private readonly HttpClient _httpClient;

		public UsersDataService(UsersHttpClient httpClient)
		{
			_httpClient = httpClient.HttpClient;
		}

		public async Task<UserInfo?> GetUser(Guid userId)
		{
            string uri = $"api/users/{userId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
				return null;
            }

            UserInfo? userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

            return userInfo;
        }

		public async Task<UserInfo> GetUserOrThrow(Guid userId)
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
