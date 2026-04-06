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
using System.Runtime.CompilerServices;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class RemovePriorityFromQueueEntryUseCaseTests
    {
        private readonly RemovePriorityFromQueueEntryUseCase _removePriorityFromQueueEntryUseCase;

        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;
        private readonly Mock<IListingReadService> _listingReadServiceMock;

        private readonly IFixture _fixture;

        public RemovePriorityFromQueueEntryUseCaseTests()
        {
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();
            _listingReadServiceMock = new Mock<IListingReadService>();

            _removePriorityFromQueueEntryUseCase = new RemovePriorityFromQueueEntryUseCase(_listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object, _listingReadServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingQueueEntry);

            // Act
            Func<Task> action = async () => await _removePriorityFromQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotFound_ThrowsListingNotFoundException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _removePriorityFromQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

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
            Func<Task> action = async () => await _removePriorityFromQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_Returns()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, queueEntry.ListingId)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act

            Func<Task> action = async () => await _removePriorityFromQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), listing.SellerUserId);

            // Assert
            await action.Should().NotThrowAsync();

            _listingQueueEntryWriteServiceMock.Verify(s => s.UpdateListingQueueEntryAsync(It.Is<ListingQueueEntry>(qe => qe == queueEntry)), Times.Once);
        }
    }
}
