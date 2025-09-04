using BoardGameMarket.UsersMicroservice.Core.Application.ServiceContracts;
using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;
using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;

namespace BoardGameMarket.UsersService.Core.Application.Services
{
	public class UserGetterService : IUserGetterService
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IGraphService _graphService;
		private readonly ILogger<UserGetterService> _logger;
		private readonly string? _adminId;

		public UserGetterService(IUsersRepository usersRepository, IGraphService graphService, ILogger<UserGetterService> logger, IConfiguration configuration)
		{
			_usersRepository = usersRepository;
			_graphService = graphService;
			_logger = logger;
			_adminId = configuration["INITIAL_ENTRA_ADMIN_ID"];
		}

		public async Task<ApplicationUser?> GetUserByUserIdAsync(Guid userId)
		{
			User? entraUser = await _graphService.GetUserByUserIdAsync(userId);
			if (entraUser == null)
				return null;

			ApplicationUser? localUser = await _usersRepository.GetUserById(userId);
			if (localUser != null)
			{
				return localUser;
			}

			_logger.LogInformation($"{nameof(UserGetterService)}:{nameof(GetUserByUserIdAsync)}: EntraId found, creating database entry");
			UserRole role = userId.ToString() == _adminId ? UserRole.Admin : UserRole.NormalUser;
			localUser = new ApplicationUser() { UserId = userId, Role = role, FirstName = entraUser.GivenName, LastName = entraUser.Surname, DisplayName = entraUser.DisplayName };

			return await _usersRepository.AddUser(localUser);
		}
	}
}
