using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	internal class CurrentUserContext : ICurrentUserContext
	{
		public Guid UserId { get; }

		public IReadOnlyCollection<string> Roles { get; } = Array.Empty<string>();

		public void Set(Guid userId, IEnumerable<string> roles)
		{
			throw new NotImplementedException();
		}
	}
}
