using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using System.Text.Json;

namespace BoardGameMarket.UsersService.Infrastructure.InMemorySeeding
{
	public class UsersSeeder : ISeeder
	{
		private readonly IUserRepository _repository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly JsonSerializerOptions _jsonOptions;

		public UsersSeeder(IUserRepository userRepository, IUnitOfWork unitOfWork, JsonSerializerOptions jsonOptions)
		{
			_repository = userRepository;
			_unitOfWork = unitOfWork;
			_jsonOptions = jsonOptions;
		}

		public async Task SeedAsync(string seedPath)
		{
			string json = File.ReadAllText(Path.Combine(seedPath, "USERS_MOCK_DATA.json"));

			IEnumerable<ApplicationUser>? users = JsonSerializer.Deserialize<IEnumerable<ApplicationUser>>(json, _jsonOptions);

			if (users == null)
			{
				return;
			}
			foreach (ApplicationUser user in users)
			{
				await _repository.AddUserAsync(user);
			}
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
