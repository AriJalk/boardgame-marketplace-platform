using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	public interface IUserRoleService
	{
		Task<UserRole?> AssignRoleToUserAsync(Guid userId, RoleType roleType);
		Task<bool> RemoveRoleFromUserAsync(Guid userId, RoleType roleType);
		Task<bool> RemoveAllRolesFromUserAsync(Guid userId);
		Task<IEnumerable<Guid>> GetUserIdsByRoleAsync(RoleType roleType);
		Task<UserRole?> GetUserRoleByUserRoleId(Guid userRoleId);
		Task<bool> UserHasRoleAsync(Guid userId, RoleType roleType);
	}
}
