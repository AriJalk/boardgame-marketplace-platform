using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;
using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;

namespace BoardGameMarket.UsersService.Core.Application.Services
{
	internal class UserDeleterService : IUserDeleterService
	{
		private readonly IUserAuthorizationService _authorizationService;
		private readonly IUsersRepository _usersRepository;

		public UserDeleterService(IUserAuthorizationService authorizationService, IUsersRepository usersRepository)
		{
			_authorizationService = authorizationService;
			_usersRepository = usersRepository;
		}

		public async Task<bool> DeleteUserAsync(Guid targetUserId, Guid requesterId)
		{
			if (targetUserId == requesterId)
			{
				throw new ArgumentException("Can't delete own user", nameof(targetUserId));
			}
			await _authorizationService.AuthorizeAsync(requesterId, UserRole.Admin);

			return await _usersRepository.DeleteUser(targetUserId);
		}
	}
}
