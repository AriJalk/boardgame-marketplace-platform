using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.AggregateBuilders
{
    public class ListingGroupAggregateBuilderTests
    {
        private readonly ListingGroupAggregateBuilder _listingGroupAggregateBuilder;

        private readonly ListingAggregateBuilder _listingAggregateBuilder;
        private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

        // Shared
        private readonly Mock<IListingReadService> _listingReadServiceMock;

        // QueueBuilder
        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;

        // ListingBuilder
        private readonly Mock<IListingItemReadService> _listingItemReadServiceMock;

        // ListingGroupBuilder
        private readonly Mock<IListingGroupReadService> _listingGroupReadServiceMock;

        private readonly IFixture _fixture;

        public ListingGroupAggregateBuilderTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingItemReadServiceMock = new Mock<IListingItemReadService>();
            _listingGroupReadServiceMock = new Mock<IListingGroupReadService>();


            _listingQueueAggregateBuilder = new ListingQueueAggregateBuilder(_listingQueueEntryReadServiceMock.Object);

            _listingAggregateBuilder = new ListingAggregateBuilder(_listingReadServiceMock.Object, _listingItemReadServiceMock.Object, _listingQueueAggregateBuilder);

            _listingGroupAggregateBuilder = new ListingGroupAggregateBuilder(_listingReadServiceMock.Object, _listingGroupReadServiceMock.Object, _listingAggregateBuilder, new Mock<ILogger<ListingGroupAggregateBuilder>>().Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetListingGroupAggregateAsync_ListingGroupMissing_ThrowsKeyNotFoundException()
        {
            // Arrange
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingGroup);

            // Act
            Func<Task> action = async () => await _listingGroupAggregateBuilder.GetListingGroupAggregateAsync(Guid.NewGuid(), false, false);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetListingGroupAggregateAsync_ListingGroupExists_ReturnsListingGroupAggregate()
        {
            // Arrange
            ListingGroup listingGroup = _fixture.Create<ListingGroup>();
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listingGroup);

            IEnumerable<Listing> listings = _fixture.Build<Listing>()
                .With(l => l.ListingGroupId, listingGroup.Id)
                .With(l => l.SellerUserId, listingGroup.SellerUserId)
                .CreateMany(3);
            _listingReadServiceMock.Setup(s => s.GetListingsByListingGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync(listings);

            Dictionary<Guid, IEnumerable<ListingItem>> listingItems = new Dictionary<Guid, IEnumerable<ListingItem>>();
            Dictionary<Guid, IEnumerable<ListingQueueEntry>> listingQueues = new Dictionary<Guid, IEnumerable<ListingQueueEntry>>();

            foreach (Listing listing in listings)
            {
                listingItems[listing.Id] = AggregateBuildersHelpers.BuildListingItems(listing.Id, _fixture);
                listingQueues[listing.Id] = AggregateBuildersHelpers.BuildQueue(listing.Id, _fixture);
            }
            _listingItemReadServiceMock.Setup(s => s.GetListingItemsByListingIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid listingId) => listingItems[listingId]);
            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync((ListingQueueEntryFilter filter) =>
            {
                Guid listingId = filter.ListingIds!.First();
                return listingQueues[listingId];
            });

            ListingGroupAggregate expectedListingGroupAggregate = new ListingGroupAggregate(listingGroup,
                listings.Select(l => new ListingAggregate(l,
                listingItems[l.Id].ToList(),
                AggregateBuildersHelpers.BuildQueueAggregate(listingQueues[l.Id]))).ToList());
            
            // Act
            ListingGroupAggregate actualListingGroupAggregateFromBuilder = await _listingGroupAggregateBuilder.GetListingGroupAggregateAsync(listingGroup.Id, true, true);

            // Assert
            actualListingGroupAggregateFromBuilder.Should().BeEquivalentTo(expectedListingGroupAggregate);
        }
    }
}
