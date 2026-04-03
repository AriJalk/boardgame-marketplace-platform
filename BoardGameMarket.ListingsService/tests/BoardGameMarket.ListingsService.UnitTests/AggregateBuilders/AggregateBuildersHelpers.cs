using AutoFixture;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.AggregateBuilders
{
    internal static class AggregateBuildersHelpers
    {
        public static ListingQueueAggregate BuildQueueAggregate(IEnumerable<ListingQueueEntry> entries)
        {
            var queueByTime = entries
                .OrderBy(qe => qe.CreatedAt)
                .ToList()
                .AsReadOnly();

            var queueByPriority = entries
                .Where(qe => qe.Priority != null)
                .OrderBy(qe => qe.Priority)
                .ToList()
                .AsReadOnly();

            return new ListingQueueAggregate(queueByTime, queueByPriority);
        }

        public static IEnumerable<ListingQueueEntry> BuildQueue(Guid listingId, IFixture fixture)
        {
            List<ListingQueueEntry> queue = fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listingId)
                .With(qe => qe.Priority, (decimal?)null)
                .CreateMany(3)
                .ToList();
            queue.AddRange(fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listingId)
                .With(qe => qe.Priority, () => fixture.Create<decimal>())
                .CreateMany(3));
            return queue;
        }

        public static IEnumerable<ListingItem> BuildListingItems(Guid listingId, IFixture fixture)
        {
            return fixture.Build<ListingItem>()
                .With(i => i.ListingId, listingId)
                .CreateMany(3);
        }
    }
}
