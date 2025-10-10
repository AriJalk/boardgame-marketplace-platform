using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class DeleteUserUseCase
	{
		private readonly IUserDeleterService _userDeleterService;
		private readonly IUserGetterService _userGetterService;

		public DeleteUserUseCase(IUserDeleterService userDeleterService, IUserGetterService userGetterService)
		{
			_userDeleterService = userDeleterService;
			_userGetterService = userGetterService;
		}

		public async Task<bool> ExecuteAsync(Guid requesterId, Guid userToDeleteId)
		{
			ApplicationUser? requester = await _userGetterService.GetUserByUserIdAsync(requesterId);
			if (requester == null || requester.Role < RoleType.Moderator)
			{
				throw new UnauthorizedAccessException();
			}
			if (requester.UserId == userToDeleteId)
			{
				throw new ArgumentException("Can't delete own user");
			}

			return await _userDeleterService.DeleteUserAsync(userToDeleteId);
		}
	}
}
