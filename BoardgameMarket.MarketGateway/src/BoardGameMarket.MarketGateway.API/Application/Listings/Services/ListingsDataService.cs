using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Application.Shared;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Services
{
	public class ListingsDataService
	{
		private readonly HttpClient _httpClient;

		public ListingsDataService(ListingsHttpClient httpClient)
		{
			_httpClient = httpClient.HttpClient;
		}

		public async Task<ListingOwnershipInfo> GetListingOwnershipInfo(Guid listingId)
		{
			string uri = $"api/listings/{listingId}/ownership";
			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			ListingOwnershipInfo? ownership = await response.Content.ReadFromJsonAsync<ListingOwnershipInfo>();

			return ownership ?? throw new InvalidDataException();
		}

		public async Task<ListingDetail> GetListing(Guid listingId)
		{
			string uri = $"api/listings/{listingId}";
			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			ListingDetail? listingDetail = await response.Content.ReadFromJsonAsync<ListingDetail>();

			return listingDetail ?? throw new InvalidDataException();
		}

	}
}
