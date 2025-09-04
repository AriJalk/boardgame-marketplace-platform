using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	internal interface IUserDeleterService
	{
		Task<bool> DeleteUserAsync(Guid targetUserId, Guid requesterId);
	}
}
