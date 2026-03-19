using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using BoardGameMarket.MarketGateway.API.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services
{
    public class ListingGroupsDataService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginService _loginService;
        private readonly RedisCacheService _cacheService;

        public ListingGroupsDataService(ListingsHttpClient httpClient, LoginService loginService, RedisCacheService redisCacheService)
        {
            _httpClient = httpClient.HttpClient;
            _loginService = loginService;
            _cacheService = redisCacheService;
        }

        public async Task<PagedResult<ListingGroupSummary>> GetListingGroupsSummaryPage(PagedListingGroupQuery query)
        {
            var filterJson = JsonSerializer.Serialize(query.Filter ?? new ListingGroupFilter());
            var filterHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(filterJson)));

            string cacheKey = $"lg-page:{query.PageSize}:{query.Cursor ?? "start"}:{filterHash}";

            PagedResult<ListingGroupSummary>? cachedPage = await _cacheService.GetCachedAsync<PagedResult<ListingGroupSummary>>(cacheKey);

            if (cachedPage != null)
            {
                return cachedPage;
            }

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

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            await _cacheService.WriteToCacheAsync(cacheKey, pagedResult, TimeSpan.FromSeconds(15));

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

            ListingGroupSummary[]? listingGroup = await response.Content.ReadFromJsonAsync<ListingGroupSummary[]>();

            return listingGroup ?? throw new InvalidDataException();
        }

        public async Task<ListingGroupDetail> GetListingGroupDetail(Guid listingGroupId)
        {
            string cacheKey = $"lg-detail:{listingGroupId}";
            ListingGroupDetail? cachedListingGroup = await _cacheService.GetCachedAsync<ListingGroupDetail>(cacheKey);

            if (cachedListingGroup != null)
            {
                return cachedListingGroup;
            }

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

            await _cacheService.WriteToCacheAsync(cacheKey, listingGroup, TimeSpan.FromSeconds(30));

            return listingGroup;
        }

        public async Task<Guid> AddListingGroup(CreateListingGroupRequest request, Guid idempotencyKey)
        {
            string idempotencyCacheKey = $"idempotency:{idempotencyKey}";
            var (acquired, existing) = await _cacheService.TryStartIdempotentAsync(idempotencyCacheKey);
            if (!acquired)
            {
                if (existing != null && Guid.TryParse(existing, out Guid existingId))
                {
                    return existingId;
                }

                throw new InvalidOperationException("IdempotencyKey still processing");
            }

            string uri = $"api/listing-groups";

            HttpRequestMessage httpRequest = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, request.SellerUserId, request);

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            Guid? result = await response.Content.ReadFromJsonAsync<Guid>();

            if (result is not Guid id)
            {
                throw new InvalidDataException();
            }

            await _cacheService.CompleteIdempotentAsync(idempotencyCacheKey, id);

            return id;
        }

        //public async Task<ListingGroupOwnershipInfo> GetListingGroupOwnershipInfo(Guid listingGroupId)
        //{
        //    string uri = $"api/listing-groups/{listingGroupId}/ownership";
        //    HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //    response.EnsureSuccessStatusCode();

        //    ListingGroupOwnershipInfo? ownership = await response.Content.ReadFromJsonAsync<ListingGroupOwnershipInfo>();

        //    return ownership ?? throw new InvalidDataException();
        //}

        public async Task<Guid> UpdateListingGroup(UpdateListingGroupRequest request)
        {
            Guid requesterUserId = await _loginService.GetUserIdOrThrow();

            string uri = $"api/listing-groups/{request.Id}";
            HttpRequestMessage httpRequest = HttpClientHelpers.BuildRequest(HttpMethod.Put, uri, requesterUserId, request);

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            Guid? result = await response.Content.ReadFromJsonAsync<Guid>();

            if (result is not Guid id)
            {
                throw new InvalidDataException();
            }

            await _cacheService.RemoveKey($"lg-detail:{request.Id}");

            return id;
        }

        public async Task<ListingGroupMetadataInfo> GetListingGroupMetadata(Guid listingGroupId)
        {
            string cacheKey = $"lg-metadata:${listingGroupId}";
            ListingGroupMetadataInfo? cachedMetadata = await  _cacheService.GetCachedAsync<ListingGroupMetadataInfo>(cacheKey);
            if (cachedMetadata != null)
            {
                return cachedMetadata;
            }

            string uri = $"api/listing-groups/{listingGroupId}/metadata";

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            ListingGroupMetadataInfo? metadata = await response.Content.ReadFromJsonAsync<ListingGroupMetadataInfo>();

            if (metadata == null)
            {
                throw new InvalidDataException();
            }

            await _cacheService.WriteToCacheAsync(cacheKey, metadata, TimeSpan.FromSeconds(30));

            return metadata;
        }
    }
}
