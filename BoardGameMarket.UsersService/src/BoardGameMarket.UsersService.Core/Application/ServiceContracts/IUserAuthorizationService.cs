using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	internal interface IUserAuthorizationService
	{
		/// <summary>
		/// Fetches the requester and ensures they have at least one of the valid roles.
		/// Throws UnauthorizedAccessException if not authorized.
		/// </summary>
		Task<ApplicationUser> AuthorizeAsync(Guid requesterId, params UserRole[] validRoles);

		/// <summary>
		/// Just checks if the requester has one of the valid roles.
		/// Returns true/false without throwing.
		/// </summary>
		Task<bool> IsUserAuthorizedAsync(Guid requesterId, params UserRole[] validRoles);
	}
}
