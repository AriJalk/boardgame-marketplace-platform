using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.AggregateBuilders
{
    public class ListingAggregateBuilderTests
    {
        private readonly ListingAggregateBuilder _listingAggregateBuilder;
        private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

        // Shared
        private readonly Mock<IListingReadService> _listingReadServiceMock;
        
        // QueueBuilder
        private readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;

        // ListingBuilder
        private readonly Mock<IListingItemReadService> _listingItemReadServiceMock;

        private readonly IFixture _fixture;

        public ListingAggregateBuilderTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingItemReadServiceMock = new Mock<IListingItemReadService>();

            _listingQueueAggregateBuilder = new ListingQueueAggregateBuilder(_listingQueueEntryReadServiceMock.Object);

            _listingAggregateBuilder = new ListingAggregateBuilder(_listingReadServiceMock.Object, _listingItemReadServiceMock.Object, _listingQueueAggregateBuilder);

            _fixture = new Fixture();
        }

       [Fact]
       public async Task BuildListingAggregateAsync_ItemAndQueueExist_ReturnsAggregate()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();
            IEnumerable<ListingItem> items = AggregateBuildersHelpers.BuildListingItems(listing.Id, _fixture);
            _listingItemReadServiceMock.Setup(s => s.GetListingItemsByListingIdAsync(It.IsAny<Guid>())).ReturnsAsync(items);
            IEnumerable<ListingQueueEntry> queue = AggregateBuildersHelpers.BuildQueue(listing.Id, _fixture);
            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);

            ListingAggregate expectedListingAggregate = new ListingAggregate(listing, items.ToList(), AggregateBuildersHelpers.BuildQueueAggregate(queue));

            // Act
            ListingAggregate actualListingAggregateFromBuilder = await _listingAggregateBuilder.BuildListingAggregateAsync(listing, true, true);

            // Assert
            actualListingAggregateFromBuilder.Should().BeEquivalentTo(expectedListingAggregate);
        }

        [Fact]
        public async Task GetListingAggregatesByListingGroupIdAsync_ListingsExist_ReturnsAggregate()
        {
            // Arrange
            Guid listingGroupId = Guid.NewGuid();
            
            IEnumerable<Listing> listings = _fixture.Build<Listing>()
                .With(l => l.ListingGroupId, listingGroupId)
                .CreateMany(3);

            _listingReadServiceMock.Setup(s => s.GetListingsByListingGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync(listings);

            Dictionary<Guid, IEnumerable<ListingQueueEntry>> queues = new Dictionary<Guid, IEnumerable<ListingQueueEntry>>();
            Dictionary<Guid, IEnumerable<ListingItem>> items = new Dictionary<Guid, IEnumerable<ListingItem>>();
            List<ListingAggregate> expectedListingAggregates = new List<ListingAggregate>();
            foreach(Listing listing in listings)
            {
                IEnumerable<ListingItem> listingItems = AggregateBuildersHelpers.BuildListingItems(listing.Id, _fixture);
                items[listing.Id] = listingItems; 
                IEnumerable<ListingQueueEntry> queue = AggregateBuildersHelpers.BuildQueue(listing.Id, _fixture);
                queues[listing.Id] = queue;
                expectedListingAggregates.Add(new ListingAggregate(listing, listingItems.ToList(), AggregateBuildersHelpers.BuildQueueAggregate(queue)));
            }
            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync((ListingQueueEntryFilter filter) =>
            {
                Guid listingId = filter.ListingIds!.First();
                return queues[listingId];
            });
            _listingItemReadServiceMock.Setup(s => s.GetListingItemsByListingIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid listingId) => items[listingId]);


            // Act
            IEnumerable<ListingAggregate> actualListingAggregatesFromBuilder = await _listingAggregateBuilder.GetListingAggregatesByListingGroupIdAsync(listingGroupId, true, true);

            // Assert
            actualListingAggregatesFromBuilder.Should().BeEquivalentTo(expectedListingAggregates);
        }
    }
}
