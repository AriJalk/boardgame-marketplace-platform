using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.Core.Application.Users
{
	public class UserServices
	{
		public UserDto? GetUserDtoFromIdentity(ClaimsPrincipal claimsPrincipal)
		{ 
			if (!Guid.TryParse(claimsPrincipal.FindFirst("oid")?.Value, out Guid id))
			{
				return null;
			}
			return new UserDto()
			{
				
			};
		}
	}
}
