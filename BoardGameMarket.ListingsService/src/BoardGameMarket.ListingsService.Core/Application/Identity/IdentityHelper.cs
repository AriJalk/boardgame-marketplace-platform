using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	internal static class IdentityHelper
	{
		public static void AuthorizeCurrentUserOrAdmin(Guid userId, ICurrentUserContext currentUserContext)
		{
			bool isAuthorized = userId == currentUserContext.UserId || !currentUserContext.Roles.Contains(RoleType.Admin.ToString());
			if (!isAuthorized)
			{
				throw new AuthorizationException("Not authorized");
			}
		}
	}
}
