using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.UsersService.Core.Application.Services
{
	internal class UserRoleService : IUserRoleService
	{
		private readonly IUserRoleRepository _userRoleRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ILogger<UserRoleService> _logger;

		public UserRoleService(IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, ILogger<UserRoleService> logger)
		{
			_userRoleRepository = userRoleRepository;
			_roleRepository = roleRepository;
			_logger = logger;
		}

		public async Task<UserRole?> AssignRoleToUserAsync(Guid userId, RoleType roleType)
		{
			ApplicationRole? role = await _roleRepository.GetRoleByRoleTypeAsync(roleType);
			if (role == null)
			{
				return null;
			}
			UserRole userRole = new UserRole()
			{
				UserRoleId = Guid.NewGuid(),
				RoleId = role.RoleId,
				UserId = userId,
				AssignedAt = DateTime.UtcNow,
			};

			return await _userRoleRepository.AddUserRoleAsync(userRole);
		}

		public async Task<IEnumerable<Guid>> GetUserIdsByRoleAsync(RoleType roleType)
		{
			ApplicationRole? role = await _roleRepository.GetRoleByRoleTypeAsync(roleType);
			if (role == null)
			{
				return Enumerable.Empty<Guid>();
			}

			return await _userRoleRepository.GetUserIdsByRoleIdAsync(role.RoleId);
		}

		public async Task<UserRole?> GetUserRoleByUserRoleId(Guid userRoleId)
		{
			return await _userRoleRepository.GetUserRoleByUserRoleIdAsync(userRoleId);
		}

		public async Task<bool> RemoveAllRolesFromUserAsync(Guid userId)
		{
			return await _userRoleRepository.DeleteAllUserRolesFromUserAsync(userId);
		}

		public async Task<bool> RemoveRoleFromUserAsync(Guid userId, RoleType roleType)
		{
			IEnumerable<UserRole> userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId);
			UserRole? userRole = userRoles.FirstOrDefault(ur => ur.Role.RoleType == roleType);
			if (userRole == null) 
			{
				return false;
			}
			return await _userRoleRepository.DeleteUserRoleAsync(userRole.UserRoleId);
		}

		public async Task<bool> UserHasRoleAsync(Guid userId, RoleType roleType)
		{
			ApplicationRole? role = await _roleRepository.GetRoleByRoleTypeAsync(roleType);
			if (role == null)
			{
				return false;
			}

			return await _userRoleRepository.UserHasRoleAsync(userId, role.RoleId);
		}
	}
}
