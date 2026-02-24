using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients;
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

	}
}
