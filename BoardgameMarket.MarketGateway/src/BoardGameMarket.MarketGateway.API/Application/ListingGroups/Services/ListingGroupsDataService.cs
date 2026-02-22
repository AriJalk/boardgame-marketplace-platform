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

		public async Task<PagedResult<ListingGroupSummary>> GetListingGroupsSummaryPage(PagedListingGroupQuery query)
		{

			string uri = $"api/listing-groups/page";

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, query);

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

		public async Task<ListingGroupSummary[]> GetFilteredListingGroupSummaries(ListingGroupFilter filter)
		{
			string uri = $"api/listing-groups/filtered";

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, filter);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new KeyNotFoundException();
			}

			response.EnsureSuccessStatusCode();

			ListingGroupSummary[]? result = await response.Content.ReadFromJsonAsync<ListingGroupSummary[]>();

			if (result == null)
			{
				throw new InvalidDataException();
			}
			return result;
		}

		public async Task<ListingGroupDetail> GetListingGroupDetail(Guid listingGroupId)
		{
			string uri = $"api/listing-groups/{listingGroupId}";

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
