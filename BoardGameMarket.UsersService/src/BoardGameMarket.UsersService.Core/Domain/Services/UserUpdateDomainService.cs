using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Domain.Services
{
	public class UserUpdateDomainService
	{
		public void ValidateRoleAssignment(RoleType requesterRole, RoleType userRole, RoleType targetRole)
		{
			if (requesterRole < userRole || requesterRole < targetRole)
			{
				throw new UnauthorizedAccessException("Higher role required");
			}
		}
	}
}
