using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	public interface ICurrentUserContext
	{
		Guid? UserId { get; }
		RoleType? Role { get; }
		void Set(Guid? userId, RoleType? role);
		bool HasUser();
	}
}
