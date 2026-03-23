using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases
{
    public class UpdateListingGroupUseCaseTests
    {
        private readonly UpdateListingGroupUseCase _updateListingGroupUseCase;

        private readonly Mock<IListingGroupReadService> _listingGroupReadServiceMock;
        private readonly Mock<IListingGroupWriteService> _listingGroupWriteServiceMock;
        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingWriteService> _listingWriteServiceMock;
        private readonly Mock<IListingItemReadService> _listingItemReadServiceMock;
        private readonly Mock<IListingItemWriteService> _listingItemWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;

        private readonly IFixture _fixture;

        public UpdateListingGroupUseCaseTests()
        {
            _listingGroupReadServiceMock = new Mock<IListingGroupReadService>();
            _listingGroupWriteServiceMock = new Mock<IListingGroupWriteService>();
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingWriteServiceMock = new Mock<IListingWriteService>();
            _listingItemReadServiceMock = new Mock<IListingItemReadService>();
            _listingItemWriteServiceMock = new Mock<IListingItemWriteService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();

            _updateListingGroupUseCase = new UpdateListingGroupUseCase(_listingGroupReadServiceMock.Object, _listingGroupWriteServiceMock.Object, _listingReadServiceMock.Object, _listingWriteServiceMock.Object, _listingItemReadServiceMock.Object, _listingItemWriteServiceMock.Object, _unitOfWorkMock.Object, _listingQueueEntryWriteServiceMock.Object, new Mock<ILogger<UpdateListingGroupUseCase>>().Object);

            _fixture = new Fixture();

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());
        }


        [Fact]
        public async Task UpdateListingGroupUseCase_MissingListingGroup_ThrowsKeyNotFoundException()
        {
            // Arrange
            Guid requesterId = Guid.NewGuid();
            UpdateListingGroupRequest request = _fixture.Create<UpdateListingGroupRequest>();
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingGroup);
            // Act
            Func<Task> action = async () => await _updateListingGroupUseCase.ExecuteAsync(request, requesterId);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task UpdateListingGroupUseCase_RequesterMismatch_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            ListingGroup listingGroup = _fixture.Create<ListingGroup>();
            Guid requesterId = Guid.NewGuid();
            UpdateListingGroupRequest request = _fixture.Build<UpdateListingGroupRequest>().With(r => r.Id, listingGroup.Id).Create();
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.Is<Guid>(id => id == listingGroup.Id))).ReturnsAsync(listingGroup);
            // Act
            Func<Task> action = async () => await _updateListingGroupUseCase.ExecuteAsync(request, requesterId);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task UpdateListingGroupUseCase_ValidRequest_UpdateFailed_ThrowsInvalidOperationException()
        {
            // Arrange
            ListingGroup listingGroup = _fixture.Create<ListingGroup>();
            UpdateListingGroupRequest request = _fixture.Build<UpdateListingGroupRequest>().With(r => r.Id, listingGroup.Id).Create();
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.Is<Guid>(id => id == listingGroup.Id))).ReturnsAsync(listingGroup);
            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).ThrowsAsync(new InvalidOperationException());
            // Act
            Func<Task> action = async () => await _updateListingGroupUseCase.ExecuteAsync(request, listingGroup.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpdateListingGroupUseCase_ValidRequest_UpdateSuccessful_ReturnsId()
        {
            // Arrange
            Guid listingGroupId = Guid.NewGuid();

            Dictionary<Guid, Listing> listings = _fixture.Build<Listing>()
                .With(l => l.ListingGroupId, listingGroupId)
                .With(l => l.Status, ListingStatus.Active)
                .CreateMany(3)
                .ToDictionary(l => l.Id);
            Dictionary<Guid, List<ListingItem>> listingItemsByListings = listings.ToDictionary(l => l.Key, l => 
            _fixture.Build<ListingItem>()
            .With(li => li.ListingId, l.Value.Id)
            .CreateMany(3)
            .ToList());
            ListingGroup listingGroup = _fixture.Build<ListingGroup>().With(lg => lg.Id, listingGroupId).Create();

            Dictionary<Guid, List<UpdateListingItemRequest>> listingItemRequests = listingItemsByListings.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                    .Select(li => _fixture.Build<UpdateListingItemRequest>()
                    .With(req => req.Id, li.Id)
                    .Create()).ToList()
                );

            List<UpdateListingRequest> listingRequests = listings.Select(l =>
            {
                return _fixture.Build<UpdateListingRequest>()
                    .With(req => req.Id, l.Value.Id)
                    .With(req => req.ListingItems, listingItemRequests[l.Key].ToArray())
                    .Create();
            }).ToList();

            UpdateListingGroupRequest listingGroupRequest = _fixture.Build<UpdateListingGroupRequest>()
                .With(req => req.Id, listingGroupId)
                .With(req => req.Listings, listingRequests.ToArray())
                .Create();


            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listingGroup);
            _listingReadServiceMock.Setup(s => s.GetListingsByListingGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync(listings.Values);
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid listingId) => listings[listingId]);
            _listingItemReadServiceMock.Setup(s => s.GetListingItemsByListingIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid listingId) => listingItemsByListings.TryGetValue(listingId, out var items) ? items : new List<ListingItem>());

            // Act
            Guid actualListingGroupIdFromUseCase = await _updateListingGroupUseCase.ExecuteAsync(listingGroupRequest, listingGroup.SellerUserId);

            // Assert 
            actualListingGroupIdFromUseCase.Should().Be(listingGroupId);
        }
    }
}
