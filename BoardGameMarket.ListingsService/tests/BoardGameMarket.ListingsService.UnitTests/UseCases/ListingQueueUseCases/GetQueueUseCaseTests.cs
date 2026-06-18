using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class GetQueueUseCaseTests
    {
        private readonly GetQueueUseCase _getQueueUseCase;
        private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingQueueEntryReadService> _listingQueueReadServiceMock;

        private readonly IFixture _fixture;

        public GetQueueUseCaseTests()
        {
            _listingQueueReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingQueueAggregateBuilder = new ListingQueueAggregateBuilder(_listingQueueReadServiceMock.Object);
            _listingReadServiceMock = new Mock<IListingReadService>();

            _getQueueUseCase = new GetQueueUseCase(_listingQueueAggregateBuilder, _listingReadServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotFound_ThrowsListingNotFoundException()
        {
            // Arrange
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _getQueueUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUser_ThrowsUnauthorizedException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _getQueueUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_ReturnsQueueInfo()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            List<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.Priority, null as decimal?)
                .With(qe => qe.ListingId, listing.Id)
                .CreateMany(10).ToList();
            queue.AddRange(_fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .CreateMany(5));

            _listingQueueReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            ListingQueueAggregate listingQueueAggregate = new ListingQueueAggregate(
                queue.OrderBy(qe => qe.CreatedAt).ToList(), queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority).ToList());

            ListingQueueInfo expectedListingQueueInfo = new ListingQueueInfo(listingQueueAggregate.QueueByTime.Select(qe => qe.ToListingQueueEntryInfo()).ToArray(), listingQueueAggregate.QueueByPriority.Select(qe => qe.ToListingQueueEntryInfo()).ToArray());

            // Act
            ListingQueueInfo actualQueueInfoFromUseCase = await _getQueueUseCase.ExecuteAsync(listing.Id, listing.SellerUserId);

            // Assert
            actualQueueInfoFromUseCase.Should().BeEquivalentTo(expectedListingQueueInfo);
        }
    }
}
