using BoardGameMarket.UsersService.Core.Application.Commands;
using BoardGameMarket.UsersService.Core.Application.Contracts;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CreateUserUseCase> _logger;

        public CreateUserUseCase(IUnitOfWork unitOfWork, IUserGetterService userGetterService, IUserAdderService userAdderService, ILogger<CreateUserUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _userGetterService = userGetterService;
            _userAdderService = userAdderService;
            _logger = logger;
        }

        private async Task<UserInfo> CreateUser(CreateUserRequest createUserRequest)
        {
            // Check existence
            ApplicationUser? existingUser = await _userGetterService.GetUserByUserIdAsync(createUserRequest.Id);
            if (existingUser != null)
            {
                _logger.LogInformation("{Service}/{Method}: User {userId} exists, returning user", nameof(CreateUserUseCase), nameof(CreateUser), createUserRequest.Id);
                //throw new InvalidOperationException("User exists");
                return existingUser.ToUserInfo();
            }
            // Add user
            CreateUserCommand command = new CreateUserCommand(createUserRequest.Id, createUserRequest.FirstName, createUserRequest.LastName, createUserRequest.DisplayName, createUserRequest.Email);
            ApplicationUser? newUser = await _userAdderService.AddUserAsync(command);
            if (newUser == null)
            {
                _logger.LogError("{Service}/{Method}: Can't create user {UserId}", nameof(CreateUserUseCase), nameof(CreateUser), createUserRequest.Id);
                throw new InvalidOperationException("Can't create user");
            }
            _logger.LogInformation("{Service}/{Method}: User {userId} created, returning user", nameof(CreateUserUseCase), nameof(CreateUser), createUserRequest.Id);
            return newUser.ToUserInfo();
        }

        public async Task<UserInfo> ExecuteAsync(CreateUserRequest createUserRequest)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(() => CreateUser(createUserRequest));
        }
    }
}
