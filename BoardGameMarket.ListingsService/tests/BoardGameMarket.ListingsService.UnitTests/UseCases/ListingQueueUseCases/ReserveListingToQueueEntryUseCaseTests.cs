using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
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
    public class ReserveListingToQueueEntryUseCaseTests
    {
        private readonly ReserveListingToQueueEntryUseCase _reserveListingToQueueEntryUseCase;

        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingWriteService> _listingWriteServiceMock;
        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;
        private readonly Mock<IListingReservationReadService> _listingReservationReadServiceMock;
        private readonly Mock<IListingReservationWriteService> _listingReservationWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly IFixture _fixture;

        public ReserveListingToQueueEntryUseCaseTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingWriteServiceMock = new Mock<IListingWriteService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();
            _listingReservationReadServiceMock = new Mock<IListingReservationReadService>();
            _listingReservationWriteServiceMock = new Mock<IListingReservationWriteService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _reserveListingToQueueEntryUseCase = new ReserveListingToQueueEntryUseCase(_listingReadServiceMock.Object, _listingWriteServiceMock.Object, _listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object, _listingReservationReadServiceMock.Object, _listingReservationWriteServiceMock.Object, _unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_MissingListing_ThrowsListingNotFoundException()
        {
            // Arrange
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _reserveListingToQueueEntryUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotActive_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Reserved)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_ReservationExistsForActiveListingForDifferentQueueEntry_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            ListingReservation existingReservation = _fixture.Build<ListingReservation>()
                .With(r => r.ListingId, listing.Id)
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByListingId(It.IsAny<Guid>())).ReturnsAsync(existingReservation);

            // Act
            Func<Task> action = async () => await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), listing.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ReservationExistsForActiveListingForSameQueueEntry_ReturnsQueueEntryId()
        {
            // Arrange
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            Guid listingQueueEntryId = Guid.NewGuid();

            ListingReservation existingReservation = _fixture.Build<ListingReservation>()
                .With(r => r.ListingId, listing.Id)
                .With(r => r.Status, ReservationStatus.Reserved)
                .With(r => r.ListingQueueEntryId, listingQueueEntryId)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByListingId(It.IsAny<Guid>())).ReturnsAsync(existingReservation);

            // Act
            Guid reservationId = await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, listingQueueEntryId, listing.SellerUserId);

            // Assert
            reservationId.Should().Be(existingReservation.Id);
        }

        [Fact]
        public async Task ExecuteAsync_EntryNotTopOfQueue_ThrowsInvalidOperationException()
        {
            // Arrange 
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .CreateMany(5);
            IEnumerable<ListingQueueEntry> orderedPriorityQueue = queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority);
            ListingQueueEntry targetQueueEntry = orderedPriorityQueue.ElementAt(1);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            // Act
            Func<Task> action = async () => await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, targetQueueEntry.Id, listing.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_ReturnsReservationId()
        {
            // Arrange 
            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Status, ListingStatus.Active)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .With(qe => qe.Status, QueueStatus.Queued)
                .CreateMany(5);
            IEnumerable<ListingQueueEntry> orderedPriorityQueue = queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority);
            ListingQueueEntry targetQueueEntry = orderedPriorityQueue.ElementAt(0);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            ListingReservation? capturedReservation = null;
            _listingReservationWriteServiceMock.Setup(s => s.AddListingReservationAsync(It.IsAny<ListingReservation>())).Callback<ListingReservation>(c => capturedReservation = c).Returns(Task.CompletedTask);

            // Act
            Guid reservationId = await _reserveListingToQueueEntryUseCase.ExecuteAsync(listing.Id, targetQueueEntry.Id, listing.SellerUserId);

            // Assert
            capturedReservation.Should().NotBeNull();
            reservationId.Should().Be(capturedReservation.Id);
        }
    }
}
