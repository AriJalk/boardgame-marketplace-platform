using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingSales.ServiceClients;
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

        public async Task<ListingSaleInfo> ApproveSellToReservation(Guid reservationId)
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/reservations/{reservationId}/sell";

            HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            ListingSaleInfo? soldInfo = await response.Content.ReadFromJsonAsync<ListingSaleInfo>();

            return soldInfo ?? throw new InvalidDataException();
        }

        public async Task CancelReservation(Guid reservationId)
        {
            Guid requesterUserId = _loginService.GetUserIdOrThrow();

            string uri = $"api/reservations/{reservationId}/cancel";

            HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
