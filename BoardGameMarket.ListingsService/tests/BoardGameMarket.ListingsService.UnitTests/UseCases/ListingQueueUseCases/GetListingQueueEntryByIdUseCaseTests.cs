using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class GetListingQueueEntryByIdUseCaseTests : GetListingQueueEntryTestsBase
    {
        private readonly GetListingQueueEntryByIdUseCase _getListingQueueEntryByIdUseCase;

        public GetListingQueueEntryByIdUseCaseTests() : base()
        {
            _getListingQueueEntryByIdUseCase = new GetListingQueueEntryByIdUseCase(_listingQueueEntryReadServiceMock.Object, _listingReadServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_QueueEntryNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingQueueEntry);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByIdUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotFound_ThrowsListingNotFoundException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByIdUseCase.ExecuteAsync(Guid.NewGuid(), queueEntry.Id, Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidRequester_ThrowsUnauthorizedException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByIdUseCase.ExecuteAsync(listing.Id, queueEntry.Id, Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsSeller_ReturnsQueueEntry()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            ListingQueueEntryInfo expectedQueueEntryInfo = queueEntry.ToListingQueueEntryInfo();

            // Act
            ListingQueueEntryInfo actualListingQueueInfo = await _getListingQueueEntryByIdUseCase.ExecuteAsync(listing.Id, queueEntry.Id, listing.SellerUserId);

            // Assert
            actualListingQueueInfo.Should().BeEquivalentTo(expectedQueueEntryInfo);
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsBuyer_ReturnsQueueEntry()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            ListingQueueEntryInfo expectedQueueEntryInfo = queueEntry.ToListingQueueEntryInfo();

            // Act
            ListingQueueEntryInfo actualListingQueueInfo = await _getListingQueueEntryByIdUseCase.ExecuteAsync(listing.Id, queueEntry.Id, queueEntry.BuyerUserId);

            // Assert
            actualListingQueueInfo.Should().BeEquivalentTo(expectedQueueEntryInfo);
        }
    }
}
