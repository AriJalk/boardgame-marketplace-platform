using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	internal interface IUserAuthorizationService
	{
		/// <summary>
		/// Fetches the requester and ensures they have at least one of the valid roles.
		/// Throws UnauthorizedAccessException if not authorized.
		/// </summary>
		Task<ApplicationUser> AuthorizeAsync(Guid requesterId, params RoleType[] validRoles);

		/// <summary>
		/// Just checks if the requester has one of the valid roles.
		/// Returns true/false without throwing.
		/// </summary>
		Task<bool> IsUserAuthorizedAsync(Guid requesterId, params RoleType[] validRoles);
	}
}
