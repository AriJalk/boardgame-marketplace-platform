using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Core.Application.Commands
{
	public record UpdateUserCommand(Guid UserId, string? FirstName, string? LastName, string DisplayName, Guid RequesterId)
	{
	}
}
