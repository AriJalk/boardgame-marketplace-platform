using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services
{
	public class QueuesDataService
	{
		private readonly HttpClient _httpClient;
		private readonly LoginService _loginService;

		public QueuesDataService(ListingsHttpClient httpClient, LoginService loginService)
		{
			_httpClient = httpClient.HttpClient;
			_loginService = loginService;
		}

		public async Task<ListingQueueInfo> GetQueueForListing(Guid listingId)
		{
			Guid requesterUserId = _loginService.GetUserIdOrThrow();
			string uri = $"api/listings/{listingId}/queue";
			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new KeyNotFoundException();
			}

			response.EnsureSuccessStatusCode();

			ListingQueueInfo? queue = await response.Content.ReadFromJsonAsync<ListingQueueInfo>();

			if (queue == null)
			{
				throw new InvalidDataException();
			}
			return queue;
		}

		public async Task<ListingQueueEntryInfo?> GetListingSubscriptionByUserAsync(Guid listingId, Guid subscriberUserId)
		{

			Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/user/{subscriberUserId}";
			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			if (response.StatusCode == HttpStatusCode.NoContent)
			{
				return null;
			}

			return await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>();
		}

		public async Task<Guid> SubscribeToListingAsync(Guid listingId)
		{

			Guid userId = _loginService.GetUserIdOrThrow();

            string uri = $"api/listings/{listingId}/queue/subscription";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, userId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Guid? queueEntryId = await response.Content.ReadFromJsonAsync<Guid>();

			return queueEntryId ?? throw new InvalidDataException();
		}

		public async Task UnsubscribeFromListingAsync(Guid listingId, Guid subscriptionId)
		{
			Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/{subscriptionId}";
			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Delete, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();
		}

		public async Task<Guid[]> GetSubscribedListingIdsAsync(Guid userId)
		{
			Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/subscriptions/users/{userId}/listings";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);
			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Guid[] listingIds = await response.Content.ReadFromJsonAsync<Guid[]>() ?? throw new InvalidOperationException("Listing Ids payload expected");

			return listingIds;
		}

		public async Task<ListingQueueEntryInfo> GetListingQueueEntryByIdAsync(Guid listingId, Guid listingQueueEntryId)
		{
			Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Get, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo queueEntry = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>() ?? throw new InvalidOperationException($"{nameof(ListingQueueEntryInfo)} payload expected");

			return queueEntry;
		}

        public async Task DeleteListingQueueEntryAsync(Guid listingId, Guid listingQueueEntryId)
        {
			Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}";
			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Delete, uri, requesterUserId);

            HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ListingQueueEntryInfo[]> GetFilteredQueueEntries(ListingQueueEntryFilter filter)
		{
            Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/subscriptions/query";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, requesterUserId, filter);
			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo[] queueEntries = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo[]>() ?? throw new InvalidOperationException("Listing Ids payload expected");

			return queueEntries;
		}

		public async Task<Guid> AddPriorityToQueueEntry(Guid listingId, Guid listingQueueEntryId)
		{
            Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Post, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Guid? queueEntryFromUpdateId = await response.Content.ReadFromJsonAsync<Guid>();

			return queueEntryFromUpdateId ?? throw new InvalidDataException();
		}


		public async Task<Guid> UpdateListingQueueEntryPriorityAsync(Guid listingId, Guid listingQueueEntryId, UpdateListingQueueEntryPriorityRequestDto requestDto)
		{
            Guid requesterUserId = _loginService.GetUserIdOrThrow();
            UpdateListingQueueEntryPriorityRequestDto updateRequest = new UpdateListingQueueEntryPriorityRequestDto(requestDto.RequestedIndex);

			string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Patch, uri, requesterUserId, updateRequest);
			
			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Guid? queueEntryFromUpdateId = await response.Content.ReadFromJsonAsync<Guid>();

			return queueEntryFromUpdateId ?? throw new InvalidDataException();
		}

		public async Task<Guid> RemoveListingQueueEntryPriorityAsync(Guid listingId, Guid listingQueueEntryId)
		{
            Guid requesterUserId = _loginService.GetUserIdOrThrow();
            string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";

			HttpRequestMessage request = HttpClientHelpers.BuildRequest(HttpMethod.Delete, uri, requesterUserId);

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Guid? listingQueueEntryIdFromResponse = await response.Content.ReadFromJsonAsync<Guid>();

			return listingQueueEntryIdFromResponse ?? throw new InvalidDataException();
		}

	}
}
