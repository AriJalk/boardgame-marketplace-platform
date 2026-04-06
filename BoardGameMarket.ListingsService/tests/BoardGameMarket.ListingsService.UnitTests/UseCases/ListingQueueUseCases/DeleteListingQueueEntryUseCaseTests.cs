using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class DeleteListingQueueEntryUseCaseTests
    {
        private readonly DeleteListingQueueEntryUseCase _deleteListingQueueEntryUseCase;

        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;
        private readonly Mock<IListingReadService> _listingReadServiceMock;

        private readonly IFixture _fixture;

        public DeleteListingQueueEntryUseCaseTests()
        {
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();
            _listingReadServiceMock = new Mock<IListingReadService>();

            _deleteListingQueueEntryUseCase = new DeleteListingQueueEntryUseCase(_listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object, _listingReadServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryNotFound_Returns()
        {
            // Arrange
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingQueueEntry);

            // Act
            Func<Task> action = async () => await _deleteListingQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotFound_ThrowsListingNotFoundException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _deleteListingQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidRequester_ThrowsUnauthorizedException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, queueEntry.ListingId)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _deleteListingQueueEntryUseCase.ExecuteAsync(listing.Id, queueEntry.Id, Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsSeller_Returns()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, queueEntry.ListingId)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _deleteListingQueueEntryUseCase.ExecuteAsync(listing.Id, queueEntry.Id, listing.SellerUserId);

            // Assert
            await action.Should().NotThrowAsync();

            _listingQueueEntryWriteServiceMock.Verify(s => s.DeleteListingQueueEntryAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsBuyer_Returns()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, queueEntry.ListingId)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _deleteListingQueueEntryUseCase.ExecuteAsync(listing.Id, queueEntry.Id, queueEntry.BuyerUserId);

            // Assert
            await action.Should().NotThrowAsync();

            _listingQueueEntryWriteServiceMock.Verify(s => s.DeleteListingQueueEntryAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
