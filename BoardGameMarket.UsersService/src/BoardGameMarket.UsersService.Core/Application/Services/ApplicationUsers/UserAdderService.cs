using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	public class UserAdderService : IUserAdderService
	{
		private readonly Guid _moderatorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
		private readonly Guid _adminId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7");
		private ILogger<UserAdderService> _logger;
		private readonly IUserRepository _userRepository;

		public UserAdderService(ILogger<UserAdderService> logger, IUserRepository userRepository)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<ApplicationUser?> AddUserAsync(ApplicationUser user)
		{
			if (user.UserId == _adminId)
			{
				user.Role = RoleType.Admin;
			}
			else if (user.UserId == _moderatorId)
			{
				user.Role = RoleType.Moderator;
			}

			return await _userRepository.AddUserAsync(user);
		}
	}
}
