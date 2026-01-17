using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	public class UserGetterService : IUserGetterService
	{
		private readonly IUserRepository _repository;

		public UserGetterService(IUserRepository usersRepository, ILogger<UserGetterService> logger)
		{
			_repository = usersRepository;
		}

		public async Task<ApplicationUser?> GetUserByUserIdAsync(Guid userId)
		{
			return await _repository.GetUserByIdAsync(userId);
		}
	}
}
