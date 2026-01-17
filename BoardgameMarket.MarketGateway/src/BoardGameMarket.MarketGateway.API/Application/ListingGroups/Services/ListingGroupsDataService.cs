using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services
{
	public class ListingGroupsDataService
	{
		private readonly HttpClient _httpClient;

		public ListingGroupsDataService(ListingsHttpClient httpClient)
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

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new KeyNotFoundException();
			}

			response.EnsureSuccessStatusCode();

			PagedResult<ListingGroupSummary>? pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<ListingGroupSummary>>();

			if (pagedResult == null)
			{
				throw new InvalidDataException();
			}
			return pagedResult;
		}

		public async Task<ListingGroupDetail> GetListingGroupDetail(Guid listingGroupId)
		{
			string uri = $"api/ListingGroups/{listingGroupId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new KeyNotFoundException();
			}

			response.EnsureSuccessStatusCode();

			ListingGroupDetail? listingGroup = await response.Content.ReadFromJsonAsync<ListingGroupDetail>();

			if (listingGroup == null)
			{
				throw new InvalidDataException();
			}
			return listingGroup;
		}
	}
}
