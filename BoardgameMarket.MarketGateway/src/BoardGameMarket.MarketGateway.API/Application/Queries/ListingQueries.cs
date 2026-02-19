using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.Shared;

namespace BoardGameMarket.MarketGateway.API.Application.Queries
{
    public class ListingQueries
    {
        private readonly HttpClient _httpClient;

        public ListingQueries(ListingsHttpClient listingHttpClient)
        {
            _httpClient = listingHttpClient.HttpClient;
        }

        public async Task<Guid> GetSellerUserIdByListingGroupId(Guid listingGroupId)
        {
            string uri = $"api/query/seller/byListingGroup/{listingGroupId}";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            Guid sellerId = await response.Content.ReadFromJsonAsync<Guid>();
            return sellerId;
        }

        public async Task<Guid> GetSellerUserIdByListingId(Guid listingId)
        {
            string uri = $"api/query/seller/byListing/{listingId}";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            Guid sellerId = await response.Content.ReadFromJsonAsync<Guid>();
            return sellerId;
        }
    }
}
