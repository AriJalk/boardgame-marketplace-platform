using AutoFixture;
using BoardGameMarket.UsersService.Core.Application.Commands;
using BoardGameMarket.UsersService.Core.Application.Contracts;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.UseCases;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameMarket.UsersService.ServiceUnitTests
{
    public class CreateUserUseCaseTests
    {
        private readonly CreateUserUseCase _createUserUseCase;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserGetterService _userGetterService;
        private readonly IUserAdderService _userAdderService;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IUserAdderService> _userAdderServiceMock;

        private readonly IFixture _fixture;

        public CreateUserUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWork = _unitOfWorkMock.Object;
            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task<UserInfo>>>())).Returns((Func<Task<UserInfo>> action) => action());

            _userGetterServiceMock = new Mock<IUserGetterService>();
            _userGetterService = _userGetterServiceMock.Object;

            _userAdderServiceMock = new Mock<IUserAdderService>();
            _userAdderService = _userAdderServiceMock.Object;

            _createUserUseCase = new CreateUserUseCase(_unitOfWork, _userGetterService, _userAdderService, new Mock<ILogger<CreateUserUseCase>>().Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateUserUseCase_ExecuteAsync_UserExists_ReturnsExistingUser()
        {
            // Arrange
            ApplicationUser existingUser = _fixture.Create<ApplicationUser>();

            _userGetterServiceMock.Setup(s => s.GetUserByUserIdAsync(It.Is<Guid>(id => id == existingUser.Id))).ReturnsAsync(existingUser);

            CreateUserRequest request = _fixture.Build<CreateUserRequest>().With(r => r.Id, existingUser.Id).Create();

            UserInfo expectedUserInfo = existingUser.ToUserInfo();

            // Act
            UserInfo userInfoFromUseCase = await _createUserUseCase.ExecuteAsync(request);

            // Assert
            userInfoFromUseCase.Should().BeEquivalentTo(expectedUserInfo);
        }

        [Fact]
        public async Task CreateUserUseCase_ExecuteAsync_AddFailure_ThrowsInvalidOperationException()
        {
            // Arrange
            _userAdderServiceMock.Setup(s => s.AddUserAsync(It.IsAny<CreateUserCommand>())).ThrowsAsync(new DbUpdateException());
            _userGetterServiceMock.Setup(s => s.GetUserByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ApplicationUser);


            CreateUserRequest request = _fixture.Create<CreateUserRequest>();

            // Act
            Func<Task> action = async () =>
            {
                await _createUserUseCase.ExecuteAsync(request);
            };

            // Assert
            await action.Should().ThrowAsync<DbUpdateException>();
        }

        [Fact]
        public async Task CreateUserUseCase_ExecuteAsync_AddSuccessful_ReturnsUserInfo()
        {
            // Arrange
            ApplicationUser newUser = _fixture.Create<ApplicationUser>();

            _userGetterServiceMock.Setup(s => s.GetUserByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ApplicationUser);
            _userAdderServiceMock.Setup(s => s.AddUserAsync(It.IsAny<CreateUserCommand>())).ReturnsAsync(newUser);

            CreateUserRequest createUserRequest = new CreateUserRequest(newUser.Id, newUser.FirstName, newUser.LastName, newUser.DisplayName, newUser.Email);

            UserInfo expectedUserInfo = newUser.ToUserInfo();

            // Act
            UserInfo? userInfoFromUseCase = await _createUserUseCase.ExecuteAsync(createUserRequest);

            // Assert
            userInfoFromUseCase.Should().BeEquivalentTo(expectedUserInfo);
        }

    }
}
