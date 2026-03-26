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

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingGroupUseCases
{
    public class AddListingGroupUseCaseTests
    {
        private readonly AddListingGroupUseCase _addListingGroupUseCase;

        private readonly Mock<IListingGroupWriteService> _listingGroupWriteServiceMock;
        private readonly Mock<IListingWriteService> _listingWriteServiceMock;
        private readonly Mock<IListingItemWriteService> _listingItemWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly IFixture _fixture;

        public AddListingGroupUseCaseTests()
        {
            _listingGroupWriteServiceMock = new Mock<IListingGroupWriteService>();

            _listingWriteServiceMock = new Mock<IListingWriteService>();

            _listingItemWriteServiceMock = new Mock<IListingItemWriteService>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());

            _addListingGroupUseCase = new AddListingGroupUseCase(_listingGroupWriteServiceMock.Object, _listingWriteServiceMock.Object, _listingItemWriteServiceMock.Object, _unitOfWorkMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUser_ThrowsUnauthorizedAccessException()
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
        public async Task ExecuteAsync_Valid_AddFailure_ThrowsInvalidOperationException()
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
        public async Task ExecuteAsync_Valid_AddSuccessful_ReturnListingGroupId()
        {
            // Arrange
            CreateListingGroupRequest request = _fixture.Create<CreateListingGroupRequest>();

            ListingGroup? capturedListingGroup = null;
            IEnumerable<Listing>? capturedListings = null;
            IEnumerable<ListingItem>? capturedListingItems = null;

            _listingGroupWriteServiceMock.Setup(s => s.AddListingGroup(It.IsAny<ListingGroup>())).Callback<ListingGroup>(c => capturedListingGroup = c).Returns(Task.CompletedTask);
            _listingWriteServiceMock.Setup(s => s.AddListingsAsync(It.IsAny<IEnumerable<Listing>>())).Callback<IEnumerable<Listing>>(c => capturedListings = c).Returns(Task.CompletedTask);
            _listingItemWriteServiceMock.Setup(s => s.AddListingItemsAsync(It.IsAny<IEnumerable<ListingItem>>())).Callback<IEnumerable<ListingItem>>(c => capturedListingItems = c).Returns(Task.CompletedTask);


            // Act
            Guid actualIdFromUseCase = await _addListingGroupUseCase.ExecuteAsync(request, request.SellerUserId);

            // Assert
            actualIdFromUseCase.Should().NotBe(Guid.Empty);
            capturedListingGroup.Should().NotBeNull();
            capturedListingGroup.Id.Should().Be(actualIdFromUseCase);
            capturedListings.Should().NotBeNull().And.NotBeEmpty();
            capturedListingItems.Should().NotBeNull().And.NotBeEmpty();

            capturedListings!.All(l => l.ListingGroupId == capturedListingGroup!.Id).Should().BeTrue();
            capturedListingItems!.All(li => capturedListings.Select(l => l.Id).Contains(li.ListingId)).Should().BeTrue();
        }
    }
}
