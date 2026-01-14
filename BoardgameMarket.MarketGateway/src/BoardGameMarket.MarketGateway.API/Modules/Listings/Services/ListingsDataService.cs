using BoardGameMarket.MarketGateway.API.Modules.Common;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Filters;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Services
{
	public class ListingsDataService
	{
		private readonly HttpClient _httpClient;

		public ListingsDataService(ListingsHttpClient httpClient)
		{
			_httpClient = httpClient.HttpClient;
		}

		public async Task<PagedResult<ListingGroupSummary>> GetListingGroupsSummaryPage(int pageSize = 10, string? cursor = null, ListingGroupFilter? filter = null)
		{
			NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

			query["pageSize"] = pageSize.ToString();
			if (cursor != null)
			{
				query["cursor"] = cursor;
			}
			if (filter?.SellerUserId != null)
			{
				query["sellerUserId"] = filter.SellerUserId.ToString();
			}


			string uri = $"api/listingGroups?{query.ToString()}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException();
			}

			PagedResult<ListingGroupSummary>? pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<ListingGroupSummary>>();

			if (pagedResult == null)
			{
				throw new InvalidDataException();
			}
			return pagedResult;
		}
	}
}
