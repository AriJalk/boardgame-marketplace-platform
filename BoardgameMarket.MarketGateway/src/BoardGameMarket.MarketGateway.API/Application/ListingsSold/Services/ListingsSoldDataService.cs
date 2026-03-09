using BoardGameMarket.MarketGateway.API.Application.ListingsSold.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingsSold.Services
{
    public class ListingsSoldDataService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginService _loginService;

        public ListingsSoldDataService(ListingsHttpClient httpClient, LoginService logicService)
        {
            _httpClient = httpClient.HttpClient;
            _loginService = logicService;
        }

        public async Task<ListingSoldInfo> GetListingSoldAsync(Guid listingSoldId)
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/listings-sold/{listingSoldId}";

            HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            ListingSoldInfo? listingSold = await response.Content.ReadFromJsonAsync<ListingSoldInfo>();

            return listingSold ?? throw new InvalidDataException();
        }

        public async Task<ListingSoldInfo[]> GetUserListingsSoldAsync()
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/listings-sold/by-user/{requesterUserId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            ListingSoldInfo[]? listingSold = await response.Content.ReadFromJsonAsync<ListingSoldInfo[]>();

            return listingSold ?? throw new InvalidDataException();
        }
    }
}
