using BoardGameMarket.MarketGateway.API.Application.ListingSales.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingSales.Services
{
    public class ListingSalesDataService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginService _loginService;

        public ListingSalesDataService(ListingsHttpClient httpClient, LoginService logicService)
        {
            _httpClient = httpClient.HttpClient;
            _loginService = logicService;
        }

        public async Task<ListingSaleInfo> GetListingSaleAsync(Guid listingSaleId)
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/listing-sales/{listingSaleId}";

            HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            ListingSaleInfo? listingSale = await response.Content.ReadFromJsonAsync<ListingSaleInfo>();

            return listingSale ?? throw new InvalidDataException();
        }

        public async Task<ListingSaleInfo[]> GetUserListingSalesAsync()
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/listing-sales/by-user/{requesterUserId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            ListingSaleInfo[]? listingSale = await response.Content.ReadFromJsonAsync<ListingSaleInfo[]>();

            return listingSale ?? throw new InvalidDataException();
        }
    }
}
