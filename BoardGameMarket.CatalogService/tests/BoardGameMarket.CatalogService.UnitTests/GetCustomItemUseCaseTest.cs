using AutoFixture;
using BoardGameMarket.CatalogService.Core.Application.Commands;
using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Application.UseCases;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.UnitTests
{
    public class GetCustomItemUseCaseTest
    {
        private readonly GetCustomItemUseCase _getCustomItemUseCase;

        private readonly ICatalogItemGetterService _catalogItemGetterService;
        private readonly ICatalogItemAdderService _catalogItemAdderService;

        private readonly Mock<ICatalogItemGetterService> _catalogItemGetterServiceMock;
        private readonly Mock<ICatalogItemAdderService> _catalogItemAdderServiceMock;

        private readonly IFixture _fixture;

        public GetCustomItemUseCaseTest()
        {
            _catalogItemGetterServiceMock = new Mock<ICatalogItemGetterService>();
            _catalogItemGetterService = _catalogItemGetterServiceMock.Object;

            _catalogItemAdderServiceMock = new Mock<ICatalogItemAdderService>();
            _catalogItemAdderService = _catalogItemAdderServiceMock.Object;

            _getCustomItemUseCase = new GetCustomItemUseCase(_catalogItemGetterService, _catalogItemAdderService);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetCustomItemUseCase_ExecuteAsync_ItemExists_ReturnsItem()
        {
            // Arrange

            string catalogItemName = _fixture.Create<string>();
            CatalogItem existingItem = _fixture.Build<CatalogItem>().With(i => i.Name, catalogItemName).Create();

            _catalogItemGetterServiceMock.Setup(s => s.GetCustomItemByNameAsync(It.Is<string>(name => name == catalogItemName))).ReturnsAsync(existingItem);

            CatalogItemInfo expectedCatalogItemInfo = existingItem.ToCatalogItemInfo();

            // Act
            CatalogItemInfo actualCatalogItemInfoFromUseCase = await _getCustomItemUseCase.ExecuteAsync(catalogItemName);

            // Assert
            actualCatalogItemInfoFromUseCase.Should().BeEquivalentTo(expectedCatalogItemInfo);
        }

        [Fact]
        public async Task GetCustomItemUseCase_ExecuteAsync_AddFailed_ThrowsInvalidOperationException()
        {
            // Arrange
            string catalogItemName = _fixture.Create<string>();
            
            _catalogItemGetterServiceMock.Setup(s => s.GetCustomItemByNameAsync(It.IsAny<string>())).ReturnsAsync(null as CatalogItem);

            AddCustomCatalogItemCommand command = new AddCustomCatalogItemCommand(catalogItemName);

            _catalogItemAdderServiceMock.Setup(s => s.AddCustomCatalogItemAsync(It.IsAny<AddCustomCatalogItemCommand>())).ReturnsAsync(null as CatalogItem);

            // Act
            Func<Task> action = async () => await _getCustomItemUseCase.ExecuteAsync(catalogItemName);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetCustomItemUseCase_ExecuteAsync_AddSuccessful_ReturnsItem()
        {
            // Arrange
            string catalogItemName = _fixture.Create<string>();
            CatalogItem newCatalogItem = _fixture.Build<CatalogItem>().With(i => i.Name, catalogItemName).Create();

            _catalogItemGetterServiceMock.Setup(s => s.GetCustomItemByNameAsync(It.IsAny<string>())).ReturnsAsync(null as CatalogItem);

            AddCustomCatalogItemCommand command = new AddCustomCatalogItemCommand(catalogItemName);

            _catalogItemAdderServiceMock.Setup(s => s.AddCustomCatalogItemAsync(It.IsAny<AddCustomCatalogItemCommand>())).ReturnsAsync(newCatalogItem);

            CatalogItemInfo expectedCatalogItemInfo = newCatalogItem.ToCatalogItemInfo();

            // Act
            CatalogItemInfo actualCatalogItemInfoFromUseCase = await _getCustomItemUseCase.ExecuteAsync(catalogItemName);

            // Assert
            actualCatalogItemInfoFromUseCase.Should().BeEquivalentTo(expectedCatalogItemInfo);
        }
    }
}
