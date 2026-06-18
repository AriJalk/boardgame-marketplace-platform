using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.AggregateBuilders
{
    public class ListingQueueAggregateBuilderTests
    {
        private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;

        private readonly IFixture _fixture;

        public ListingQueueAggregateBuilderTests()
        {
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();

            _listingQueueAggregateBuilder = new ListingQueueAggregateBuilder(_listingQueueEntryReadServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetListingQueueAggregate_NoQueue_ReturnsEmpty()
        {
            // Arrange
            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(new List<ListingQueueEntry>());

            // Act
            ListingQueueAggregate queueFromBuilder = await _listingQueueAggregateBuilder.GetListingQueueAggregate(Guid.NewGuid());

            // Assert
            queueFromBuilder.QueueByTime.Should().BeEmpty();
            queueFromBuilder.QueueByPriority.Should().BeEmpty();
        }

        [Fact]
        public async Task GetListingQueueAggregate_QueueExists_ReturnsQueue()
        {
            // Arrange
            Guid listingId = Guid.NewGuid();
            IEnumerable<ListingQueueEntry> queue = AggregateBuildersHelpers.BuildQueue(listingId, _fixture);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            ListingQueueAggregate expectedListingQueueAggregate = AggregateBuildersHelpers.BuildQueueAggregate(queue);

            // Act
            ListingQueueAggregate actualListingQueueInfoFromBuilder = await _listingQueueAggregateBuilder.GetListingQueueAggregate(listingId);

            actualListingQueueInfoFromBuilder.Should().BeEquivalentTo(expectedListingQueueAggregate);
        }
    }
}
