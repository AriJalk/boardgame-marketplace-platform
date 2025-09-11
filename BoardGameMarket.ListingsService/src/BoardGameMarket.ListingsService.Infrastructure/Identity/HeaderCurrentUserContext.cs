using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameMarket.ListingsService.Core.Application.Identity;

namespace BoardGameMarket.ListingsService.Infrastructure.Identity
{
	internal class HeaderCurrentUserContext : ICurrentUserContext
	{
		public Guid UserId { get; private set; }
		public IReadOnlyCollection<string> Roles { get; private set; } = Array.Empty<string>();

		public void Set(Guid userId, IEnumerable<string> roles)
		{
			UserId = userId;
			Roles = roles.ToArray();
		}
	}
}
