using AutoFixture;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.DomainServices
{
    public class QueueDomainServiceTests
    {
        private readonly IFixture _fixture;

        public QueueDomainServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ValidateReserve_EmptyQueue_ThrowsInvalidOperationException()
        {
            // Act
            Action action = () => QueueDomainService.ValidateReserve(Enumerable.Empty<ListingQueueEntry>(), _fixture.Create<ListingQueueEntry>());

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidateReserve_RequestedEntryNotTopOfQueue_ThrowsInvalidOperationException()
        {
            // Arrange
            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>().CreateMany(5).OrderBy(qe => qe.Priority ?? int.MaxValue).ThenBy(qe => qe.CreatedAt);

            Action action = () => QueueDomainService.ValidateReserve(queue, queue.ElementAt(1));

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidateReserve_RequestedEntryNotInQueuedStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>().CreateMany(5).OrderBy(qe => qe.Priority ?? int.MaxValue).ThenBy(qe => qe.CreatedAt);

            ListingQueueEntry targetQueueEntry = queue.ElementAt(0);
            targetQueueEntry.Status = QueueStatus.Cancelled;

            Action action = () => QueueDomainService.ValidateReserve(queue, targetQueueEntry);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidateReserve_ValidRequest_Returns()
        {
            // Arrange
            IEnumerable<ListingQueueEntry> queue = _fixture.Build<ListingQueueEntry>().CreateMany(5).OrderBy(qe => qe.Priority ?? int.MaxValue).ThenBy(qe => qe.CreatedAt);

            ListingQueueEntry targetQueueEntry = queue.ElementAt(0);
            targetQueueEntry.Status = QueueStatus.Queued;

            Action action = () => QueueDomainService.ValidateReserve(queue, targetQueueEntry);

            action.Should().NotThrow();
        }
    }
}
