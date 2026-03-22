using AutoFixture;
using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Application.Commands;
using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Application.UseCases;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameMarket.CatalogService.UnitTests
{
    public class GetBggItemUseCaseTests
    {
        private readonly GetBggItemUseCase _getBggItemUseCase;
        
        private readonly ICatalogItemGetterService _catalogItemGetterService;
        private readonly ICatalogItemAdderService _catalogItemAdderService;
        private readonly IBggClient _bggClient;

        private readonly Mock<ICatalogItemGetterService> _catalogItemGetterServiceMock;
        private readonly Mock<ICatalogItemAdderService> _catalogItemAdderServiceMock;
        private readonly Mock<IBggClient> _bggClientMock;

        private readonly IFixture _fixture;


        public GetBggItemUseCaseTests()
        {
            _catalogItemGetterServiceMock = new Mock<ICatalogItemGetterService>();
            _catalogItemGetterService = _catalogItemGetterServiceMock.Object;

            _catalogItemAdderServiceMock = new Mock<ICatalogItemAdderService>();
            _catalogItemAdderService = _catalogItemAdderServiceMock.Object;

            _bggClientMock = new Mock<IBggClient>();
            _bggClient = _bggClientMock.Object;

            _getBggItemUseCase = new GetBggItemUseCase(_catalogItemGetterService, _catalogItemAdderService, _bggClient, new Mock<ILogger<GetBggItemUseCase>>().Object);
            
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetBggItemUseCase_ExecuteAsync_ItemExists_ReturnsItem()
        {
            // Arrange
            int bggId = 1;
            CatalogItem existingCatalogItem = _fixture.Build<CatalogItem>().With(i => i.BggId, bggId).Create();

            _catalogItemGetterServiceMock.Setup(s => s.GetBggItemByBggIdAsync(It.Is<int>(id => id == bggId))).ReturnsAsync(existingCatalogItem);

            CatalogItemInfo expectedCatalogItemInfo = existingCatalogItem.ToCatalogItemInfo();

            // Act
            CatalogItemInfo actualCatalogItemInfoFromUseCase = await _getBggItemUseCase.ExecuteAsync(bggId);

            // Assert
            actualCatalogItemInfoFromUseCase.Should().BeEquivalentTo(expectedCatalogItemInfo);
        }

        [Fact]
        public async Task GetBggItemUseCase_ExecuteAsync_MissingFromBgg_ThrowsKeyNotFoundException()
        {
            // Arrange
            int bggId = 1;
            _catalogItemGetterServiceMock.Setup(s => s.GetBggItemByBggIdAsync(It.Is<int>(id => id == bggId))).ReturnsAsync(null as CatalogItem);

            _bggClientMock.Setup(c => c.GetMultiBggMetadataAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(Enumerable.Empty<RawBggMetadata>());

            // Act
            Func<Task> action = async () => await _getBggItemUseCase.ExecuteAsync(bggId);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetBggItemUseCase_ExecuteAsync_AddFailed_ThrowsInvalidOperationException()
        {
            // Arrange
            int bggId = 1;
            _catalogItemGetterServiceMock.Setup(s => s.GetBggItemByBggIdAsync(It.Is<int>(id => id == bggId))).ReturnsAsync(null as CatalogItem);

            RawBggMetadata bggMetaData = _fixture.Create<RawBggMetadata>();
            _bggClientMock.Setup(c => c.GetMultiBggMetadataAsync(It.Is<IEnumerable<int>>(ids => ids.Contains(bggId)))).ReturnsAsync([bggMetaData]);

            AddBggCatalogItemCommand addCommand = new AddBggCatalogItemCommand(bggMetaData);
            _catalogItemAdderServiceMock.Setup(s => s.AddBggCatalogItemAsync(It.Is<AddBggCatalogItemCommand>(c => c == addCommand))).ReturnsAsync(null as CatalogItem);

            // Act
            Func<Task> action = async () => await _getBggItemUseCase.ExecuteAsync(bggId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetBggItemUseCase_ExecuteAsync_AddSuccessful_ReturnsItem()
        {
            // Arrange
            int bggId = 1;
            _catalogItemGetterServiceMock.Setup(s => s.GetBggItemByBggIdAsync(It.Is<int>(id => id == bggId))).ReturnsAsync(null as CatalogItem);

            RawBggMetadata bggMetaData = _fixture.Create<RawBggMetadata>();
            _bggClientMock.Setup(c => c.GetMultiBggMetadataAsync(It.Is<IEnumerable<int>>(ids => ids.Contains(bggId)))).ReturnsAsync([bggMetaData]);

            CatalogItem newCatalogItem = new CatalogItem() { 
                Id = Guid.NewGuid(),
                BggId = bggId,
                Categories = bggMetaData.Categories,
                Name = bggMetaData.Name,
                ModifiedTimestamp = DateTime.UtcNow,
            };

            CatalogItemInfo expectedCatalogItemInfo = newCatalogItem.ToCatalogItemInfo();

            AddBggCatalogItemCommand addCommand = new AddBggCatalogItemCommand(bggMetaData);
            _catalogItemAdderServiceMock.Setup(s => s.AddBggCatalogItemAsync(It.Is<AddBggCatalogItemCommand>(c => c == addCommand))).ReturnsAsync(newCatalogItem);

            // Act
            CatalogItemInfo actualCatalogItemInfoFromUseCase = await _getBggItemUseCase.ExecuteAsync(bggId);

            // Assert
            actualCatalogItemInfoFromUseCase.Should().BeEquivalentTo(expectedCatalogItemInfo);
        }
    }
}
