using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Domain.RepositoryContracts
{
	public interface IRoleRepository
	{
		Task<ApplicationRole?> GetRoleByRoleIdAsync(Guid roleId);
		Task<ApplicationRole?> GetRoleByRoleTypeAsync(RoleType roleType);
		Task<ApplicationRole?> GetRoleWithUserAssignmentsByRoleId(Guid roleId);
		Task<ApplicationRole?> GetRoleWithUserAssignmentsByRoleType(RoleType roleType);
		Task<ApplicationRole?> AddRoleAsync(ApplicationRole applicationRole);
	}
}
