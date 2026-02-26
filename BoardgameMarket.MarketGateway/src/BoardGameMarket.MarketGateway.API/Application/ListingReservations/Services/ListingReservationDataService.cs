using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services
{
    public class ListingReservationDataService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginService _loginService;

        public ListingReservationDataService(ListingsHttpClient httpClient, LoginService loginService)
        {
            _httpClient = httpClient.HttpClient;
            _loginService = loginService;
        }

        public async Task<ListingReservationInfo> GetListingReservation(Guid listingReservationId)
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/reservations/{listingReservationId}";
            HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            ListingReservationInfo? reservation = await response.Content.ReadFromJsonAsync<ListingReservationInfo>();

            return reservation ?? throw new InvalidDataException();
        }

        public async Task<IEnumerable<ListingReservationInfo>> GetCurrentUserListingReservations()
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/reservations/user/{requesterUserId}";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            IEnumerable<ListingReservationInfo>? reservation = await response.Content.ReadFromJsonAsync<IEnumerable<ListingReservationInfo>>();

            return reservation ?? throw new InvalidDataException();
        }
    }
}
