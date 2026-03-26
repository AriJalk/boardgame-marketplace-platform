using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class AddListingQueueEntryUseCaseTests
    {
        private AddListingQueueEntryUseCase _addListingQueueEntryUseCase;

        private Mock<IListingReadService> _listingReadServiceMock;
        private Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;

        private readonly IFixture _fixture;
        public AddListingQueueEntryUseCaseTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();

            _addListingQueueEntryUseCase = new AddListingQueueEntryUseCase(_listingReadServiceMock.Object, _listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_ListingMissing_ThrowsListingNotFoundException()
        {
            // Arrange
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid());
            };

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryUseCase.ExecuteAsync(listing.Id, listing.SellerUserId);
            };

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingInactive_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Reserved).Create();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryUseCase.ExecuteAsync(listing.Id, Guid.NewGuid());
            };

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryExists_ReturnsQueueEntry()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active).Create();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            Guid requesterId = Guid.NewGuid();
            ListingQueueEntry existingQueueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.BuyerUserId, requesterId)
                .Create();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([existingQueueEntry]);

            // Act
            ListingQueueEntry actualQueueEntryFromUseCase = await _addListingQueueEntryUseCase.ExecuteAsync(listing.Id, requesterId);

            // Assert
            actualQueueEntryFromUseCase.Should().BeEquivalentTo(existingQueueEntry);
        }

        [Fact]
        public async Task ExecuteAsync_RequestValid_ReturnsQueueEntry()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active).Create();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            Guid requesterId = Guid.NewGuid();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([]);

            ListingQueueEntry expectedQueueEntry = new ListingQueueEntry()
            {
                Id = Guid.Empty,
                ListingId = listing.Id,
                BuyerUserId = requesterId,
                Status = QueueStatus.Queued,
            };

            // Act
            ListingQueueEntry actualQueueEntryFromUseCase = await _addListingQueueEntryUseCase.ExecuteAsync(listing.Id, requesterId);

            expectedQueueEntry.Id = actualQueueEntryFromUseCase.Id;
            expectedQueueEntry.CreatedAt = actualQueueEntryFromUseCase.CreatedAt;
            expectedQueueEntry.ModifiedAt = actualQueueEntryFromUseCase.ModifiedAt;

            // Assert
            actualQueueEntryFromUseCase.Should().BeEquivalentTo(expectedQueueEntry);
        }
    }
}
