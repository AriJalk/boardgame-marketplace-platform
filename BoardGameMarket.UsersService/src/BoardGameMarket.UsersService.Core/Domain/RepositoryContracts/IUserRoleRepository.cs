using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Domain.RepositoryContracts
{
	public interface IUserRoleRepository
	{
		Task<UserRole?> AddUserRoleAsync(UserRole userRole);
		Task<bool> DeleteUserRoleAsync(Guid userRoleId);
		Task<bool> DeleteAllUserRolesFromUserAsync(Guid userId);
		Task<IEnumerable<Guid>> GetUserIdsByRoleIdAsync(Guid roleId);
		Task<UserRole?> GetUserRoleByUserRoleIdAsync(Guid userRoleId);
		Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId);
		Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
	}
}
