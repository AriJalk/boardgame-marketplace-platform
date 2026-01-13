using System.Text.Json;
using System.Text.Json.Serialization;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups;
using BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Modules.Users.Services
{
	public class UsersService
	{
		public Dictionary<Guid, UserInfo> Users = new Dictionary<Guid, UserInfo>();
		public UsersService(IWebHostEnvironment hostEnvironment, JsonSerializerOptions jsonOptions)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "USERS_MOCK_DATA.json"));

			IEnumerable<UserInfo>? users = JsonSerializer.Deserialize<IEnumerable<UserInfo>>(json, jsonOptions);
			if (users == null)
			{
				return;
			}
			foreach (UserInfo user in users)
			{
				Users[user.Id] = user;
			}
		}
	}
}
