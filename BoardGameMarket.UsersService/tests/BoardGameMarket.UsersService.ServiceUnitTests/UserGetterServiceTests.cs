using AutoFixture;
using BoardGameMarket.UsersMicroservice.Core.Application.ServiceContracts;
using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;
using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts;
using BoardGameMarket.UsersService.Core.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Moq;

namespace BoardGameMarket.UsersService.ServiceUnitTests
{
	public class UserGetterServiceTests
	{
		private readonly IUserGetterService _userGetterService;
		private readonly IFixture _fixture;
		private readonly Mock<IUsersRepository> _usersRepositoryMock;
		private readonly Mock<IGraphService> _graphServiceMock;
		private readonly Mock<ILogger<UserGetterService>> _loggerMock;
		private readonly Mock<IConfiguration> _configurationMock;

		public UserGetterServiceTests()
		{
			_fixture = new Fixture();
			_usersRepositoryMock = new Mock<IUsersRepository>();
			_graphServiceMock = new Mock<IGraphService>();
			_loggerMock = new Mock<ILogger<UserGetterService>>();
			_configurationMock = new Mock<IConfiguration>();
			_userGetterService = new UserGetterService(_usersRepositoryMock.Object, _graphServiceMock.Object, _loggerMock.Object, _configurationMock.Object);
		}

		[Fact]
		public async Task GetUserByUserIdAsync_MissingUser_ReturnsNull()
		{
			//Arrange
			Guid userId = Guid.NewGuid();

			_graphServiceMock.Setup(s => s.GetUserByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

			//Act
			ApplicationUser? user = await _userGetterService.GetUserByUserIdAsync(userId);

			//Assert
			user.Should().BeNull();
		}

		[Fact]
		public async Task GetUserByUserIdAsync_UserExistsInEntraButMissingInDatabase_CreatesUser()
		{
			//Arrange
			Guid userId = Guid.NewGuid();
			User graphUser = new User() { Id = userId.ToString(), GivenName = _fixture.Create<string>(), Surname = _fixture.Create<string>(), DisplayName = _fixture.Create<string>() };
			ApplicationUser expectedUser = new ApplicationUser() { UserId = userId, FirstName = graphUser.GivenName, LastName = graphUser.Surname, DisplayName = graphUser.DisplayName };

			_graphServiceMock.Setup(s => s.GetUserByUserIdAsync(It.Is<Guid>(g => g == userId))).ReturnsAsync(graphUser);
			_usersRepositoryMock.Setup(r => r.GetUserById(It.IsAny<Guid>())).ReturnsAsync(null as ApplicationUser);
			_usersRepositoryMock.Setup(r => r.AddUser(It.Is<ApplicationUser>(u => u.UserId == userId))).ReturnsAsync(expectedUser);
			//Act
			ApplicationUser? actualUser = await _userGetterService.GetUserByUserIdAsync(userId);

			//Assert
			actualUser.Should().BeEquivalentTo(expectedUser);
			_usersRepositoryMock.Verify(r => r.AddUser(It.IsAny<ApplicationUser>()), Times.Once);
		}

		[Fact]
		public async Task GetUserByUserIdAsync_UserExistsInBoth_ReturnsUser()
		{
			//Arrange
			Guid userId = Guid.NewGuid();
			User graphUser = new User() { Id = userId.ToString() };

			ApplicationUser expectedUser = new ApplicationUser()
			{
				UserId = userId,
				Role = UserRole.NormalUser,
			};
			_graphServiceMock.Setup(s => s.GetUserByUserIdAsync(It.Is<Guid>(g => g == userId))).ReturnsAsync(graphUser);
			_usersRepositoryMock.Setup(r => r.GetUserById(It.Is<Guid>(g => g == userId))).ReturnsAsync(expectedUser);

			//Act
			ApplicationUser? actualUser = await _userGetterService.GetUserByUserIdAsync(userId);

			//Assert
			actualUser.Should().BeEquivalentTo(expectedUser);
		}
	}
}
