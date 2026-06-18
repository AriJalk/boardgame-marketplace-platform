using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
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
        public async Task GetListingGroupAggregateAsync_ListingGroupNotFound_ThrowsKeyNotFoundException()
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

        [Fact]
        public async Task GetSummaryPageAsync_PageSizeOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int lowerPageSize = -1;
            int upperPageSize = 101;

            // Act
            Func<Task> lowerAction = async () => await _listingGroupAggregateBuilder.GetSummaryPageAsync(null, lowerPageSize, null);
            Func<Task> upperAction = async () => await _listingGroupAggregateBuilder.GetSummaryPageAsync(null, upperPageSize, null);

            // Assert
            await lowerAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
            await upperAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task GetSummaryPageAsync_ListingsInIdsFilterNotFound_ReturnsEmptyPage()
        {
            // Arrange
            IEnumerable<Listing> listings = _fixture.Build<Listing>().CreateMany(3);
            _listingReadServiceMock.Setup(s => s.GetListingsByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listings);
            Guid extraId = Guid.NewGuid();
            List<Guid> listingIds = listings.Select(s => s.Id).ToList();
            listingIds.Add(extraId);
            ListingGroupFilter filter = new ListingGroupFilter()
            {
                ListingIds = listingIds,
            };

            // Act
            PagedResult<ListingGroupAggregate> resultFromBuilder = await _listingGroupAggregateBuilder.GetSummaryPageAsync(null, 50, filter);

            // Assert
            resultFromBuilder.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSummaryPageAsync_ValidListingIdsFilter_ReturnsListingGroups()
        {
            // Arrange
            IEnumerable<ListingGroup> listingGroups = _fixture.Build<ListingGroup>().CreateMany(3);
            ListingGroupFilter? capturedFilter = null;
            _listingGroupReadServiceMock.Setup(s => s.GetListingGroupsPageAsync(It.IsAny<Cursor>(), 50, It.IsAny<ListingGroupFilter>()))
                .Callback<Cursor, int, ListingGroupFilter>((_, _, f) => capturedFilter = f)
                .ReturnsAsync(new PagedResult<ListingGroup>() { Items = listingGroups.ToArray(), NextCursor = null });

            Dictionary<Guid, List<Listing>> listings = new Dictionary<Guid, List<Listing>>();

            foreach (ListingGroup listingGroup in listingGroups)
            {
                listings[listingGroup.Id] = _fixture.Build<Listing>()
                    .With(l => l.ListingGroupId, listingGroup.Id)
                    .CreateMany(3)
                    .ToList();
            }
            _listingReadServiceMock.Setup(s => s.GetListingsByListingGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid listingGroupId) => listings[listingGroupId]);


            IEnumerable<Listing> firstListings = listings.Values.Select(val => val.First());
            _listingReadServiceMock.Setup(s => s.GetListingsByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(firstListings);

            ListingGroupFilter filter = new ListingGroupFilter()
            {
                ListingIds = firstListings.Select(l => l.Id),
            };

            // Act
            PagedResult<ListingGroupAggregate> actualPageFromBuilder = await _listingGroupAggregateBuilder.GetSummaryPageAsync(null, 50, filter);

            // Assert
            actualPageFromBuilder.Items.Should().HaveCount(listingGroups.Count());
            capturedFilter!.ListingGroupIds.Should().NotBeNull();
            capturedFilter.ListingGroupIds.Should().BeEquivalentTo(listingGroups.Select(lg => lg.Id));

        }

        [Fact]
        public async Task GetFilteredListingGroups_ListingIdsAndListingGroupsFilter_ReturnAllGroups()
        {
            // Arrange
            IEnumerable<ListingGroup> listingGroupsFromFilter = _fixture.Build<ListingGroup>().CreateMany(3);
            IEnumerable<ListingGroup> listingGroupsFromListings = _fixture.Build<ListingGroup>().CreateMany(3);
            IEnumerable<ListingGroup> allListingGroups = listingGroupsFromFilter.Concat(listingGroupsFromListings);
            List<Guid> filterListingGroupIds = listingGroupsFromFilter.Select(lg => lg.Id).ToList();
            List<Listing> filterListings = new List<Listing>();
            foreach (ListingGroup listingGroup in listingGroupsFromListings)
            {
                filterListings.AddRange(_fixture.Build<Listing>()
                    .With(l => l.ListingGroupId, listingGroup.Id)
                    .CreateMany(3));
            }
            filterListings.Add(_fixture.Build<Listing>()
                .With(l => l.ListingGroupId, listingGroupsFromFilter.First().Id)
                .Create());
            _listingReadServiceMock.Setup(s => s.GetListingsByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(filterListings);

            ListingGroupFilter filter = new ListingGroupFilter()
            {
                ListingIds = filterListings.Select(l => l.Id),
                ListingGroupIds = listingGroupsFromFilter.Select(lg => lg.Id),
            };

            ListingGroupFilter? capturedFilter = null;

            _listingGroupReadServiceMock.Setup(s => s.GetFilteredListingGroups(It.IsAny<ListingGroupFilter>())).Callback<ListingGroupFilter>((callbackFilter) => capturedFilter = callbackFilter).ReturnsAsync(allListingGroups);


            List<Guid> combinedListingGroupIds = (filter.ListingGroupIds ?? Enumerable.Empty<Guid>())
                .Concat(listingGroupsFromListings
                .Select(lg => lg.Id))
                .Distinct()
                .ToList();
            ListingGroupFilter expectedFilter = new ListingGroupFilter()
            {
                ListingIds = filter.ListingIds,
                ListingGroupIds = combinedListingGroupIds,
            };

            // Act
            IReadOnlyCollection<ListingGroupAggregate> actualListingGroupAggregatesFromBuilder = await _listingGroupAggregateBuilder.GetFilteredListingGroups(filter);

            // Assert
            actualListingGroupAggregatesFromBuilder.Should().HaveCount(allListingGroups.Count());
            capturedFilter!.ListingGroupIds.Should().BeEquivalentTo(expectedFilter.ListingGroupIds);
            capturedFilter!.ListingIds.Should().BeEquivalentTo(expectedFilter.ListingIds);
        }
    }
}
