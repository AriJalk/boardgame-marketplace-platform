using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Core.Application.Contracts
{
	public record ChangeUserRoleRequest(RoleType Type, Guid RequesterId) { }
}
