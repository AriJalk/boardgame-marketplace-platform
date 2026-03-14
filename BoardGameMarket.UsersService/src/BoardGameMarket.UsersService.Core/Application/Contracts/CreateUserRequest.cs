using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Core.Application.Contracts
{
	public record CreateUserRequest(Guid Id, string? FirstName, string? LastName, string DisplayName, string Email)
	{
	}
}
