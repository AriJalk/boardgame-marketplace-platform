using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
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
		private ILogger<UserAdderService> _logger;
		private readonly IUserRepository _userRepository;

		public UserAdderService(ILogger<UserAdderService> logger, IUserRepository userRepository)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<ApplicationUser?> AddUserAsync(ApplicationUser user)
		{
			return await _userRepository.AddUserAsync(user);
		}
	}
}
