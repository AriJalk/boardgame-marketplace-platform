using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases
{
    public class AddListingGroupUseCaseTests
    {
        private readonly AddListingGroupUseCase _addListingGroupUseCase;

        private readonly IListingGroupWriteService _listingGroupWriteService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IListingItemWriteService _listingItemWriteService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly Mock<IListingGroupWriteService> _listingGroupWriteServiceMock;
        private readonly Mock<IListingWriteService> _listingWriteServiceMock;
        private readonly Mock<IListingItemWriteService> _listingItemWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly IFixture _fixture;

        public AddListingGroupUseCaseTests()
        {
            _listingGroupWriteServiceMock = new Mock<IListingGroupWriteService>();
            _listingGroupWriteService = _listingGroupWriteServiceMock.Object;

            _listingWriteServiceMock = new Mock<IListingWriteService>();
            _listingWriteService = _listingWriteServiceMock.Object;

            _listingItemWriteServiceMock = new Mock<IListingItemWriteService>();
            _listingItemWriteService = _listingItemWriteServiceMock.Object;

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWork = _unitOfWorkMock.Object;

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());

            _addListingGroupUseCase = new AddListingGroupUseCase(_listingGroupWriteService, _listingWriteService, _listingItemWriteService, _unitOfWork);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddListingGroupUseCase_MismatchRequesterId_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            CreateListingGroupRequest request = _fixture.Create<CreateListingGroupRequest>();
            Guid requesterId = Guid.NewGuid();

            // Act
           Func<Task> action = async () => await _addListingGroupUseCase.ExecuteAsync(request, requesterId);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task AddListingGroupUseCase_Valid_AddFailure_Throws()
        {
            // Arrange
            CreateListingGroupRequest request = _fixture.Create<CreateListingGroupRequest>();
            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Throws<InvalidOperationException>();

            // Act
            Func<Task> action = async () => await _addListingGroupUseCase.ExecuteAsync(request, request.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task AddListingGroupUseCase_Valid_AddSuccessful_ReturnListingGroupId()
        {
            // Arrange
            CreateListingGroupRequest request = _fixture.Create<CreateListingGroupRequest>();

            // Act
            Guid actualIdFromUseCase = await _addListingGroupUseCase.ExecuteAsync(request, request.SellerUserId);

            actualIdFromUseCase.Should().NotBe(Guid.Empty);
        }
    }
}
