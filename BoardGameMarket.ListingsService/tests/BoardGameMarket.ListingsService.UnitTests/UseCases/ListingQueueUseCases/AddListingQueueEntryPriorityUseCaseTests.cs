using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class AddListingQueueEntryPriorityUseCaseTests
    {
        private readonly AddListingQueueEntryPriorityUseCase _addListingQueueEntryPriorityUseCase;

        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;

        private readonly IFixture _fixture;

        public AddListingQueueEntryPriorityUseCaseTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();

            _addListingQueueEntryPriorityUseCase = new AddListingQueueEntryPriorityUseCase(_listingReadServiceMock.Object, _listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object);

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
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
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
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), Guid.NewGuid());
            };

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryMissing_ThrowsKeyNotFoundException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([]);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), listing.SellerUserId);
            };

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryForAnotherListing_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([queueEntry]);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, queueEntry.Id, listing.SellerUserId);
            };

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_PriorityExists_Returns()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            ListingQueueEntry queueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .With(qe => qe.Priority, 1000)
                .Create();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([queueEntry]);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, queueEntry.Id, listing.SellerUserId);
            };

            // Assert
            await action.Should().NotThrowAsync();
        }


        [Fact]
        public async Task ExecuteAsync_PriorityAdded_Returns()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);


            ListingQueueEntry requestedQueueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .With(qe => qe.Priority, null as Decimal?)
                .Create();
            ListingQueueEntry highestPriorityQueueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .With(qe => qe.Priority, QueueDomainService.INITIAL_PRIORITY)
                .Create();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([requestedQueueEntry, highestPriorityQueueEntry]);

            ListingQueueEntry? capturedListingQueueEntry = null;

            _listingQueueEntryWriteServiceMock.Setup(s => s.UpdateListingQueueEntryAsync(It.IsAny<ListingQueueEntry>())).Callback<ListingQueueEntry>(c => capturedListingQueueEntry = c).Returns(Task.CompletedTask);

            // Act
            Func<Task> action = async () =>
            {
                await _addListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, requestedQueueEntry.Id, listing.SellerUserId);
            };

            // Assert
            await action.Should().NotThrowAsync();
            capturedListingQueueEntry.Should().NotBeNull();
            capturedListingQueueEntry.Priority.Should().Be(highestPriorityQueueEntry.Priority + QueueDomainService.PRIORITY_GAP);
        }
    }
}
