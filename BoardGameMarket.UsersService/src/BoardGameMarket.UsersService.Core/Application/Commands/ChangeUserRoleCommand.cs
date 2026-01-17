using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Core.Application.Commands
{
	public record ChangeUserRoleCommand(Guid TargetUserId, RoleType NewRole, Guid RequesterId)
	{
	}
}
