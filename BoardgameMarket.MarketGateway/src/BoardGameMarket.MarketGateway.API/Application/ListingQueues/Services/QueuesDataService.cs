using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using System.Net;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services
{
	public class QueuesDataService
	{
		private readonly HttpClient _httpClient;

		public QueuesDataService(ListingsHttpClient httpClient)
		{
			_httpClient = httpClient.HttpClient;
		}

		public async Task<ListingQueueInfo> GetQueueForListing(Guid listingId)
		{
			string uri = $"api/listings/{listingId}/queue";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

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

		public async Task<ListingQueueEntryInfo?> GetListingSubscriptionByUserAsync(Guid listingId, Guid userId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription/user/{userId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			if (response.StatusCode == HttpStatusCode.NoContent)
			{
				return null;
			}

			return await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>();
		}

		public async Task<ListingQueueEntryInfo> SubscribeToListingAsync(Guid listingId, Guid userId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription";
			AddListingQueueEntryRequest addRequest = new AddListingQueueEntryRequest(userId);
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, addRequest);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo queueEntry = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>() ??
				throw new InvalidOperationException("Expected subscription payload");

			return queueEntry;
		}

		public async Task UnsubscribeFromListingAsync(Guid listingId, Guid subscriptionId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription/{subscriptionId}";
			HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

			response.EnsureSuccessStatusCode();
		}

		public async Task<Guid[]> GetSubscribedListingIdsAsync(Guid userId)
		{
			string uri = $"api/subscriptions/users/{userId}/listings";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			Guid[] listingIds = await response.Content.ReadFromJsonAsync<Guid[]>() ?? throw new InvalidOperationException("Listing Ids payload expected");

			return listingIds;
		}

		public async Task<ListingQueueEntryInfo> GetListingQueueEntryByIdAsync(Guid listingId, Guid listingQueueEntryId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo queueEntry = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>() ?? throw new InvalidOperationException($"{nameof(ListingQueueEntryInfo)} payload expected");

			return queueEntry;
		}

        public async Task DeleteListingQueueEntryAsync(Guid listingId, Guid listingQueueEntryId)
        {
            string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}";

            HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ListingQueueEntryInfo[]> GetFilteredQueueEntries(ListingQueueEntryFilter filter)
		{
			string uri = $"api/subscriptions/query";
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, filter);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo[] queueEntries = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo[]>() ?? throw new InvalidOperationException("Listing Ids payload expected");

			return queueEntries;
		}

		public async Task<ListingQueueEntryInfo> AddPriorityToQueueEntry(Guid listingId, Guid listingQueueEntryId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";

			HttpResponseMessage response = await _httpClient.PostAsync(uri, null);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo? queueEntry = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>();

			if (queueEntry == null)
			{
				throw new InvalidOperationException($"Can't add priority to entry {listingQueueEntryId}");
			}

			return queueEntry;
		}


		public async Task<ListingQueueEntryInfo> UpdateListingQueueEntryPriorityAsync(Guid listingId, Guid listingQueueEntryId, UpdateListingQueueEntryPriorityRequestDto requestDto)
		{
			UpdateListingQueueEntryPriorityRequestDto request = new UpdateListingQueueEntryPriorityRequestDto(requestDto.RequestedIndex);

			string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";
			
			HttpResponseMessage response = await _httpClient.PatchAsJsonAsync(uri, request);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo? queueEntry = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>();

			if (queueEntry == null)
			{
				throw new InvalidOperationException($"{nameof(ListingQueueEntryInfo)} payload expected");
			}

			return queueEntry;
		}

		public async Task<ListingQueueEntryInfo> RemoveListingQueueEntryPriorityAsync(Guid listingId, Guid listingQueueEntryId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription/{listingQueueEntryId}/priority";
			HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

			response.EnsureSuccessStatusCode();

			ListingQueueEntryInfo listingQueueEntryInfo = await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>() ?? throw new InvalidOperationException($"{nameof(ListingQueueEntryInfo)} payload expectred");

			return listingQueueEntryInfo;
		}

	}
}
