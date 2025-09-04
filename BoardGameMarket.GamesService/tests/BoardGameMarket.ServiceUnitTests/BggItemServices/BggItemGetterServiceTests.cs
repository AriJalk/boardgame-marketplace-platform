using AutoFixture;
using BoardGameMarket.GamesService.Core.Application.AdapterContracts;
using BoardGameMarket.GamesService.Core.Application.ServiceContracts;
using BoardGameMarket.GamesService.Core.Application.Services;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.RepositoryContracts;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameMarket.ServiceUnitTests.BggItemServices
{
	public class BggItemGetterServiceTests
	{
		private readonly IBggItemGetterService _bggItemGetterService;
		private readonly IFixture _fixture;
		private readonly Mock<IBggItemsRepository> _bggItemsRepositoryMock;
		private readonly Mock<ICategoriesRepository> _categoriesRepositoryMock;
		private readonly Mock<IBggClient> _bggClientMock;
		private readonly Mock<ILogger<BggItemGetterService>> _loggerMock;
		private readonly Mock<IBggItemFactory> _bggItemFactoryMock;


		public BggItemGetterServiceTests()
		{
			_fixture = new Fixture();
			_bggItemsRepositoryMock = new Mock<IBggItemsRepository>();
			_categoriesRepositoryMock = new Mock<ICategoriesRepository>();
			_bggClientMock = new Mock<IBggClient>();
			_loggerMock = new Mock<ILogger<BggItemGetterService>>();
			_bggItemFactoryMock = new Mock<IBggItemFactory>();
			_bggItemGetterService = new BggItemGetterService(_bggItemsRepositoryMock.Object, _categoriesRepositoryMock.Object, _bggClientMock.Object, _loggerMock.Object, _bggItemFactoryMock.Object);
		}

		[Fact]
		public async Task GetItemsByBggId_SingleItemExistInDatabase_ReturnsItem()
		{
			//Arrange
			BggMetadata metadata = _fixture.Create<BggMetadata>();
			BggItem expectedItem = _fixture.Build<BggItem>()
				.With(g => g.BggMetadata, metadata)
				.Create();

			_bggItemsRepositoryMock.Setup(r => r.GetItemByBggIdAsync(It.Is<int>(i => i == metadata.BggId))).ReturnsAsync(expectedItem);

			//Act
			IEnumerable<BggItem> actualItemCollection = await _bggItemGetterService.GetItemsByBggIds(new List<int> { metadata.BggId });

			//Assert
			actualItemCollection.Should().ContainEquivalentOf(expectedItem);
			_bggItemsRepositoryMock.Verify(r => r.GetItemByBggIdAsync(metadata.BggId), Times.Once);
		}
	}
}
