using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	internal class CurrentUserContext : ICurrentUserContext
	{
		public Guid UserId { get; private set; }

		public IReadOnlyCollection<string> Roles { get; private set; } = new List<string>();

		public void Set(Guid userId, IEnumerable<string> roles)
		{
			UserId = userId;
			Roles = (IReadOnlyCollection<string>)roles;
		}
	}
}
