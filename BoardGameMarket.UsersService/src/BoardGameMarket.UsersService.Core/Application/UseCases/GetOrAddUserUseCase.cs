using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class GetOrAddUserUseCase
	{
		private readonly ILogger<GetOrAddUserUseCase> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGraphService _graphService;
		private readonly IUserGetterService _userGetterService;
		private readonly IUserAdderService _userAdderService;
		private readonly IUserRoleService _userRoleService;
		private readonly Guid? _adminId;

		public GetOrAddUserUseCase(ILogger<GetOrAddUserUseCase> logger, IUnitOfWork unitOfWork, IGraphService graphService, IUserGetterService userGetterService, IUserAdderService userAdderService, IUserRoleService userRoleService, IConfiguration configuration)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
			_graphService = graphService;
			_userGetterService = userGetterService;
			_userAdderService = userAdderService;
			_userRoleService = userRoleService;
			string? adminId = configuration["INITIAL_ENTRA_ADMIN_ID"];
			if (adminId != null && Guid.TryParse(adminId, out Guid adminGuid))
			{
				_adminId = adminGuid;
			}
		}

		public async Task<ApplicationUser?> ExecuteAsync(Guid userId)
		{
			User? entraUser = await _graphService.GetUserByUserIdAsync(userId);
			if (entraUser == null)
			{
				return null;
			}
			
			ApplicationUser? applicationUser = await _userGetterService.GetUserByUserIdAsync(userId);
			if (applicationUser != null)
			{
				return applicationUser;
			}
			_logger.LogInformation("{class}:{method} - entra user found but no application user found, creating", nameof(GetOrAddUserUseCase), nameof(ExecuteAsync));

			try
			{
				applicationUser = entraUser.ToApplicationUser();
				await _unitOfWork.BeginTransactionAsync();
				ApplicationUser? addedUser = await _userAdderService.AddUserAsync(applicationUser);
				RoleType roleType = userId == _adminId ? RoleType.Admin : RoleType.User;
				UserRole? userRole = await _userRoleService.AssignRoleToUserAsync(userId, roleType);
				await _unitOfWork.CommitTransactionAsync();
				return addedUser;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "{class}:{method} - Error creating user with role", nameof(GetOrAddUserUseCase), nameof(ExecuteAsync));
				return null;
			}
		}
	}
}
