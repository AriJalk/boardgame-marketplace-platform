using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using Microsoft.Graph.IdentityGovernance.EntitlementManagement.Assignments.AdditionalAccessWithAccessPackageIdWithIncompatibleAccessPackageId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class DeleteUserUseCase
	{
		private readonly IUserDeleterService _userDeleterService;
		private readonly IUserGetterService _userGetterService;

		

		public async Task<bool> ExecuteAsync(Guid requesterId, Guid userToDeleteId)
		{
			ApplicationUser? requester = await _userGetterService.GetUserByUserIdAsync(requesterId);
			if (requester == null || !requester.Role.IsAuthorized(RoleType.Moderator | RoleType.Admin))
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
