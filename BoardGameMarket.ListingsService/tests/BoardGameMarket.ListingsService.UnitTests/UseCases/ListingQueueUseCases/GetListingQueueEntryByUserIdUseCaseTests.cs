using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
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
    public class GetListingQueueEntryByUserIdUseCaseTests : GetListingQueueEntryTestsBase
    {
        private readonly GetListingQueueEntryByUserIdUseCase _getListingQueueEntryByUserIdUseCase;

        public GetListingQueueEntryByUserIdUseCaseTests() : base() 
        {
            _getListingQueueEntryByUserIdUseCase = new GetListingQueueEntryByUserIdUseCase(_listingQueueEntryReadServiceMock.Object, _listingReadServiceMock.Object);
        }


        [Fact]
        public async Task ExecuteAsync_ListingMissing_ThrowsListingNotFoundException()
        {
            // Arrange
            ListingQueueEntry queueEntry = _fixture.Create<ListingQueueEntry>();
            _listingQueueEntryReadServiceMock.Setup(s => s.GetListingQueueEntryByIdAsync(It.IsAny<Guid>())).ReturnsAsync(queueEntry);

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidRequester_ThrowsUnauthorizedException()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsSeller_Returns()
        {
            // Arrange
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listing.Id, Guid.NewGuid(), listing.SellerUserId);

            // Assert
            await action.Should().NotThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_RequesterIsBuyer_Returns()
        {
            // Arrange
            Guid buyerUserId = Guid.NewGuid();
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listing.Id, buyerUserId, buyerUserId);

            // Assert
            await action.Should().NotThrowAsync<UnauthorizedAccessException>();
        } 
        
        [Fact]
        public async Task ExecuteAsync_ValidRequest_NoQueueEntry_ReturnsNull()
        {
            // Arrange
            Guid buyerUserId = Guid.NewGuid();
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync(Enumerable.Empty<ListingQueueEntry>());


            // Act
            ListingQueueEntryInfo? actualListingQueueEntryFromUseCase = await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listing.Id, buyerUserId, buyerUserId);

            // Assert
            actualListingQueueEntryFromUseCase.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_QueueEntryExists_ReturnsQueueEntry()
        {
            // Arrange
            Guid buyerUserId = Guid.NewGuid();
            Listing listing = _fixture.Create<Listing>();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            ListingQueueEntry queueEntry = _fixture.Build<ListingQueueEntry>()
                .With(qe => qe.ListingId, listing.Id)
                .Create();

            _listingQueueEntryReadServiceMock.Setup(s => s.GetFilteredListingQueueEntriesAsync(It.IsAny<ListingQueueEntryFilter>())).ReturnsAsync([queueEntry]);

            ListingQueueEntryInfo expectedListingQueueEntryInfo = queueEntry.ToListingQueueEntryInfo();


            // Act
            ListingQueueEntryInfo? actualListingQueueEntryInfoFromUseCase = await _getListingQueueEntryByUserIdUseCase.ExecuteAsync(listing.Id, buyerUserId, buyerUserId);

            // Assert
            actualListingQueueEntryInfoFromUseCase.Should().BeEquivalentTo(expectedListingQueueEntryInfo);
        }
    }
}
