using BoardGameMarket.UsersService.Core.Application.Commands;
using BoardGameMarket.UsersService.Core.Application.Contracts;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class CreateUserUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserGetterService _userGetterService;
		private readonly IUserAdderService _userAdderService;

		public CreateUserUseCase(IUnitOfWork unitOfWork, IUserGetterService userGetterService, IUserAdderService userAdderService)
		{
			_unitOfWork = unitOfWork;
			_userGetterService = userGetterService;
			_userAdderService = userAdderService;
		}

		public async Task<UserInfo> ExecuteAsync(CreateUserRequest createUserRequest)
		{
			

			await _unitOfWork.BeginTransactionAsync();

			try
			{
				// Check existence
				ApplicationUser? existingUser = await _userGetterService.GetUserByUserIdAsync(createUserRequest.UserId);
				if (existingUser != null)
				{
					throw new InvalidOperationException("User exists");
				}
				// Add user
				CreateUserCommand command = new CreateUserCommand(createUserRequest.UserId, createUserRequest.Role, createUserRequest.FirstName, createUserRequest.LastName, createUserRequest.DisplayName, createUserRequest.Email);
				ApplicationUser? newUser = await _userAdderService.AddUserAsync(command);
				if (newUser == null)
				{
					throw new InvalidCastException("Can't create user");
				}
				await _unitOfWork.CommitTransactionAsync();
				return newUser.ToUserInfo();
			}
			catch
			{
				await _unitOfWork.RollBackTransactionAsync();
				throw;
			}
		}
	}
}
