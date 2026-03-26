using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class UpdateListingQueueEntryPriorityUseCaseTests
    {
        private readonly UpdateListingQueueEntryPriorityUseCase _updateListingQueueEntryPriorityUseCase;

        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        private readonly Mock<IListingQueueEntryWriteService> _listingQueueEntryWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;


        private readonly IFixture _fixture;

        public UpdateListingQueueEntryPriorityUseCaseTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueEntryWriteServiceMock = new Mock<IListingQueueEntryWriteService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _updateListingQueueEntryPriorityUseCase = new UpdateListingQueueEntryPriorityUseCase(_listingQueueEntryReadServiceMock.Object, _listingQueueEntryWriteServiceMock.Object, _unitOfWorkMock.Object, _listingReadServiceMock.Object);

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_ListingMissing_ThrowsListingNotFoundException()
        {
            // Arrange
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            UpdateListingQueueEntryPriorityRequest request = _fixture.Create<UpdateListingQueueEntryPriorityRequest>();

            // Act
            Func<Task> action = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), request, Guid.NewGuid());
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


            UpdateListingQueueEntryPriorityRequest request = _fixture.Create<UpdateListingQueueEntryPriorityRequest>();

            // Act
            Func<Task> action = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), request, Guid.NewGuid());
            };

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryMissing_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([]);


            UpdateListingQueueEntryPriorityRequest request = _fixture.Create<UpdateListingQueueEntryPriorityRequest>();

            // Act
            Func<Task> action = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), request, listing.SellerUserId);
            };

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryForAnotherListing_ThrowsInvalidOperationException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([queueEntry]);

            UpdateListingQueueEntryPriorityRequest request = _fixture.Create<UpdateListingQueueEntryPriorityRequest>();

            // Act
            Func<Task> action = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, queueEntry.Id, request, listing.SellerUserId);
            };

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_IndexOutOfRange_ThrowsInvalidOperationException()
        {
            Random random = new Random();
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.Is<Guid>(id => id == listing.Id))).ReturnsAsync(listing);

            ListingQueueEntry queueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .Create();
            List<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .With(qe => qe.Priority, null as decimal?)
                .CreateMany(10).ToList();
            queue.Add(queueEntry);
            int priority = QueueDomainService.INITIAL_PRIORITY;
            int priorityCount = 0;
            for (int i = 0; i < queue.Count; i++)
            {
                if (i % 2 == 0)
                {
                    queue[i].Priority = priority;
                    priority += QueueDomainService.PRIORITY_GAP;
                    priorityCount++;
                }
            }

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            // Act
            Func<Task> higherIndexAction = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, queueEntry.Id, new UpdateListingQueueEntryPriorityRequest(priorityCount), listing.SellerUserId);
            };

            Func<Task> lowerIndexAction = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, queueEntry.Id, new UpdateListingQueueEntryPriorityRequest(-1), listing.SellerUserId);
            };

            // Assert
            await higherIndexAction.Should().ThrowAsync<InvalidOperationException>();
            await lowerIndexAction.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_Returns()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();

            _listingReadServiceMock
                .Setup(s => s.GetListingByIdAsync(listing.Id))
                .ReturnsAsync(listing);

            var queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .CreateMany(5)
                .ToList();

            foreach (var (item, index) in queue.Select((v, i) => (v, i)))
            {
                item.Priority = index * 1000;
            }

            var target = queue[2];

            _listingQueueEntryReadServiceMock
                .Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>()))
                .ReturnsAsync(queue);

            List<ListingQueueEntry>? captured = null;

            _listingQueueEntryWriteServiceMock
                .Setup(s => s.UpdateListingQueueEntriesAsync(It.IsAny<IEnumerable<ListingQueueEntry>>()))
                .Callback<IEnumerable<ListingQueueEntry>>(q => captured = q.ToList())
                .Returns(Task.CompletedTask);


            // Act
            Func<Task> action = async () =>
            {
                await _updateListingQueueEntryPriorityUseCase.ExecuteAsync(listing.Id, target.Id, new UpdateListingQueueEntryPriorityRequest(0), listing.SellerUserId);
            };

            // Assert
            await action.Should().NotThrowAsync();

            captured.Should().NotBeNull();

            captured!.Count.Should().Be(queue.Count(q => q.Priority != null));

            captured[0].Id.Should().Be(target.Id);

            captured.Select(q => q.Priority).Should().BeInAscendingOrder();
        }
    }
}
