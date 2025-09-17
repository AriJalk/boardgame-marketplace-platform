using BoardGameMarket.MarketOrchestrator.BusinessLogic.DTOs.ServiceEntities;
using System.Net.Http.Json;

namespace BoardGameMarket.MarketOrchestrator.BusinessLogic.HttpClients
{
	internal class BggCatalogServiceClient
	{
		private readonly HttpClient _httpClient;

		private async Task<IEnumerable<BggItem>?> GetRequest<T>(string template, IEnumerable<T> ids)
		{
			IEnumerable<string> idStrings = ids.Select(i => string.Format(template, i));
			Uri uri = new Uri(string.Join("&", idStrings));
			HttpResponseMessage response = await _httpClient.GetAsync(uri);
			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException();
			}
			IEnumerable<BggItem>? itemResponses = await response.Content.ReadFromJsonAsync<IEnumerable<BggItem>?>();

			return itemResponses;
		}

		public async Task<IEnumerable<BggItem>?> GetBggItemsByDbId(IEnumerable<Guid> dbIds)
		{
			string template = "dbIds={id}";
			return await GetRequest(template, dbIds);
		}

		public async Task<IEnumerable<BggItem>?> GetBggItemsByBggId(IEnumerable<int> bggIds)
		{
			string template = "bggIds={id}";
			return await GetRequest(template, bggIds);
		}
	}
}
