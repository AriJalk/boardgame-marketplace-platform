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

            IEnumerable<ListingItem> listingItems = _fixture.Build<ListingItem>()
                .With(i => i.ListingId, listing.Id)
                .CreateMany(5);
            _listingItemReadServiceMock.Setup(s => s.GetListingItemsByListingIdAsync(It.IsAny<Guid>())).ReturnsAsync(listingItems);

            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .CreateMany(5);
            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(queue);
            ListingQueueAggregate listingQueueAggregate = new ListingQueueAggregate(queue.OrderBy(qe => qe.CreatedAt).ToList(), queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority).ToList());

            ListingAggregate expectedListingAggregate = new ListingAggregate(listing, listingItems.ToList().AsReadOnly(), listingQueueAggregate);

            // Act
            ListingAggregate actualListingAggregateFromBuilder = await _listingAggregateBuilder.BuildListingAggregateAsync(listing, true, true);

            // Assert
            actualListingAggregateFromBuilder.Should().BeEquivalentTo(expectedListingAggregate);
        }
    }
}
