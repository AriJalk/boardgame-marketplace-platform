using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class DeleteUserUseCase
	{
		private readonly IUserDeleterService _userDeleterService;

		public DeleteUserUseCase(IUserDeleterService userDeleterService)
		{
			_userDeleterService = userDeleterService;
		}

		public async Task<bool> ExecuteAsync(ApplicationUser requester, Guid userToDeleteId)
		{
			if (!requester.Roles.IsAuthorized(RoleType.Moderator | RoleType.Admin))
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
