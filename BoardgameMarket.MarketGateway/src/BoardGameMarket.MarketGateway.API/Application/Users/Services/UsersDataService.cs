using System.Text.Json;
using System.Text.Json.Serialization;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
	public class UsersDataService
	{
		private Dictionary<Guid, UserInfo> _users = new Dictionary<Guid, UserInfo>();
		public UsersDataService(IWebHostEnvironment hostEnvironment, JsonSerializerOptions jsonOptions)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "USERS_MOCK_DATA.json"));

			IEnumerable<UserInfo>? users = JsonSerializer.Deserialize<IEnumerable<UserInfo>>(json, jsonOptions);
			if (users == null)
			{
				return;
			}
			foreach (UserInfo user in users)
			{
				_users[user.Id] = user;
			}
		}

		public async Task<UserInfo> GetUser(Guid userId)
		{
			return _users[userId];
		}
	}
}
