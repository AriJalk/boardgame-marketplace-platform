using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients;
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

			return pagedResult ?? throw new InvalidDataException();
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

			ListingGroupSummary[]? listingGroup = await response.Content.ReadFromJsonAsync<ListingGroupSummary[]>();

			return listingGroup ?? throw new InvalidDataException();
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

			return listingGroup ?? throw new InvalidDataException();
		}

		public async Task<ListingGroupDetail> AddListingGroup(CreateListingGroupRequest request)
		{
			string uri = $"api/listing-groups";

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, request);

			response.EnsureSuccessStatusCode();

			ListingGroupDetail? listingGroup = await response.Content.ReadFromJsonAsync<ListingGroupDetail>();

			return listingGroup ?? throw new InvalidDataException();
		}

        public async Task<ListingGroupOwnershipInfo> GetListingGroupOwnershipInfo(Guid listingGroupId)
        {
            string uri = $"api/listing-groups/{listingGroupId}/ownership";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            ListingGroupOwnershipInfo? ownership = await response.Content.ReadFromJsonAsync<ListingGroupOwnershipInfo>();

            return ownership ?? throw new InvalidDataException();
        }
    }
}
