using BoardGameMarket.MarketOrchestrator.BusinessLogic.DTOs.ServiceEntities;
using System.Net.Http.Json;

namespace BoardGameMarket.MarketOrchestrator.BusinessLogic.HttpClients
{
	internal class UsersServiceClient
	{
		private readonly HttpClient _httpClient;

		public async Task<User> GetUser(Guid userId)
		{
			HttpResponseMessage httpResponse = await _httpClient.GetAsync(userId.ToString());

			if (!httpResponse.IsSuccessStatusCode)
			{
				throw new InvalidOperationException();
			}

			User? user = await httpResponse.Content.ReadFromJsonAsync<User?>();

			if (user == null)
			{
				throw new InvalidOperationException();
			}

			return user;
		}
	}
}
