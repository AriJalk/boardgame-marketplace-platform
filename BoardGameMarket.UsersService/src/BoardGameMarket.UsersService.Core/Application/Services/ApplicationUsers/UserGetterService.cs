using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	public class UserGetterService : IUserGetterService
	{
		private readonly IUserRepository _usersRepository;
		private readonly ILogger<UserGetterService> _logger;

		public UserGetterService(IUserRepository usersRepository, ILogger<UserGetterService> logger)
		{
			_usersRepository = usersRepository;
			_logger = logger;
		}

		public async Task<ApplicationUser?> GetUserByUserIdAsync(Guid userId)
		{
			_logger.LogInformation("{service}:{method} - Get user {userId}", nameof(UserGetterService), nameof(GetUserByUserIdAsync), userId);

			return await _usersRepository.GetUserWithRolesByIdAsync(userId);
		}
	}
}
