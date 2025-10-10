using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Identity
{
	internal class IdentityValidationService : IIdentityValidationService
	{
		private ICurrentUserContext _currentUserContext;

		public IdentityValidationService(ICurrentUserContext currentUserContext)
		{
			_currentUserContext = currentUserContext;
		}

		public void ValidateSameUserOrAtLeastRole(Guid userId)
		{
			if (userId != _currentUserContext.UserId && _currentUserContext.Role < RoleType.Moderator)
			{
				throw new UnauthorizedAccessException();
			}
		}
	}
}
