using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	internal class CurrentUserContext : ICurrentUserContext
	{
		public Guid? UserId { get; private set; }
		public RoleType? Role { get; private set; }


		public void Set(Guid? userId, RoleType? role)
		{
			UserId = userId;
			Role = role;
		}

		public bool HasUser()
		{
			return UserId != null && Role != null;
		}
	}


}
