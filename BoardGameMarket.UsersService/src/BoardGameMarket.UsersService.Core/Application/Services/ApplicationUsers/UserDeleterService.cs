using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	internal class UserDeleterService : IUserDeleterService
	{
		private readonly IUserAuthorizationService _authorizationService;
		private readonly IUserRepository _usersRepository;
		private readonly IUserRoleService _userRoleService;

		public UserDeleterService(IUserAuthorizationService authorizationService, IUserRepository usersRepository, IUserRoleService userRoleService)
		{
			_authorizationService = authorizationService;
			_usersRepository = usersRepository;
			_userRoleService = userRoleService;
		}

		public async Task<bool> DeleteUserAsync(Guid targetUserId, Guid requesterId)
		{
			if (targetUserId == requesterId)
			{
				throw new ArgumentException("Can't delete own user", nameof(targetUserId));
			}
			await _authorizationService.AuthorizeAsync(requesterId, RoleType.Admin);

			await _userRoleService.RemoveAllRolesFromUserAsync(targetUserId);

			return await _usersRepository.DeleteUserAsync(targetUserId);
		}
	}
}
