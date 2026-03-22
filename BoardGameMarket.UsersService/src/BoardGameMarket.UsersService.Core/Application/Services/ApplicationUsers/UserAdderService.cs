using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.Commands;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
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
		private readonly IUserRepository _repository;
		private readonly IUnitOfWork _unitOfWork;

		public UserAdderService(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_repository = userRepository;
			_unitOfWork = unitOfWork;
		}

        // Commit immediately if no transaction, or participate in a transaction if used under UnitOfWork.
        public async Task<ApplicationUser> AddUserAsync(CreateUserCommand createUserCommand)
		{
			ApplicationUser user = new ApplicationUser()
			{
				Id = createUserCommand.Id,
				FirstName = createUserCommand.FirstName,
				LastName = createUserCommand.LastName,
				DisplayName = createUserCommand.DisplayName,
				Email = createUserCommand.Email,
			};

			await _repository.AddUserAsync(user);
			await _unitOfWork.SaveChangesAsync();
			return user;

		}
	}
}
