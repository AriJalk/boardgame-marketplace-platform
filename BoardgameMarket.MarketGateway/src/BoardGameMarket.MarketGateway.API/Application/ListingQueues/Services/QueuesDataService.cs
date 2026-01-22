using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using System.Net;

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

		public async Task<ListingQueueEntryInfo?> GetSubscriptionByUserAsync(Guid listingId, Guid userId)
		{
			string uri = $"api/listings/{listingId}/queue/subscription?byUser={userId}";

			HttpResponseMessage response = await _httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			if (response.StatusCode == HttpStatusCode.NoContent)
			{
				return null;
			}

			return await response.Content.ReadFromJsonAsync<ListingQueueEntryInfo>();
		}
	}
}
