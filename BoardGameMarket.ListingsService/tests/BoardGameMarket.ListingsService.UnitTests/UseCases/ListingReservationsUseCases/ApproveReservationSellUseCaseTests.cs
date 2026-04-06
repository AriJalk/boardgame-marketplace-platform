using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingSales;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSale;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingReservationsUseCases
{
    public class ApproveReservationSellUseCaseTests
    {
        private readonly ApproveReservationSellUseCase _approveReservationSellUseCase;

        private readonly Mock<IListingReadService> _listingReadServiceMock;
        private readonly Mock<IListingWriteService> _listingWriteServiceMock;
        private readonly Mock<IListingReservationReadService> _listingReservationReadServiceMock;
        private readonly Mock<IListingReservationWriteService> _listingReservationWriteServiceMock;
        private readonly Mock<IListingSaleReadService> _listingSaleReadServiceMock;
        private readonly Mock<IListingSaleWriteService> _listingSaleWriteServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly IFixture _fixture;

        public ApproveReservationSellUseCaseTests()
        {
            _listingReadServiceMock = new Mock<IListingReadService>();
            _listingWriteServiceMock = new Mock<IListingWriteService>();
            _listingReservationReadServiceMock = new Mock<IListingReservationReadService>();
            _listingReservationWriteServiceMock = new Mock<IListingReservationWriteService>();
            _listingSaleReadServiceMock = new Mock<IListingSaleReadService>();
            _listingSaleWriteServiceMock = new Mock<IListingSaleWriteService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _approveReservationSellUseCase = new ApproveReservationSellUseCase(_listingReadServiceMock.Object, _listingWriteServiceMock.Object, _listingReservationReadServiceMock.Object, _listingReservationWriteServiceMock.Object, _listingSaleReadServiceMock.Object, _listingSaleWriteServiceMock.Object, _unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(uow => uow.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> action) => action());

            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteAsync_ReservationNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as ListingReservation);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUser_ThrowsUnauthorizedException()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();

            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ExecuteAsync_SaleExistsForReservation_ReturnsSale()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            ListingSale existingSale = _fixture.Build<ListingSale>()
                .With(s => s.ReservationId, reservation.Id)
                .Create();
            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(existingSale);

            ListingSaleInfo expectedSaleInfo = existingSale.ToListingSaleInfo();

            // Act
            ListingSaleInfo actualSaleInfoFromUseCase = await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            actualSaleInfoFromUseCase.Should().BeEquivalentTo(expectedSaleInfo);
        }

        [Fact]
        public async Task ExecuteAsync_SaleExistsForAnotherReservation_ThrowsInvalidOperationException()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            ListingSale existingSale = _fixture.Build<ListingSale>()
                .With(s => s.ReservationId, Guid.NewGuid())
                .Create();
            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(existingSale);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ReservationNotActive_ThrowsInvalidOperationException()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Cancelled)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(null as ListingSale);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotFound_ThrowsListingNotFoundException()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(null as ListingSale);

            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<ListingNotFoundException>();
        }

        [Fact]
        public async Task ExecuteAsync_ListingNotReserved_ThrowsInvalidOperationException()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(null as ListingSale);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, reservation.Id)
                .With(l => l.Status, ListingStatus.Cancelled)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            // Act
            Func<Task> action = async () => await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidRequest_ReturnsListingSaleInfo()
        {
            // Arrange
            ListingReservation reservation = _fixture.Build<ListingReservation>()
                .With(r => r.Status, ReservationStatus.Reserved)
                .Create();
            _listingReservationReadServiceMock.Setup(s => s.GetListingReservationByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);

            _listingSaleReadServiceMock.Setup(s => s.GetListingSaleByListingId(It.IsAny<Guid>())).ReturnsAsync(null as ListingSale);

            Listing listing = _fixture.Build<Listing>()
                .With(l => l.Id, reservation.ListingId)
                .With(l => l.Status, ListingStatus.Reserved)
                .Create();
            _listingReadServiceMock.Setup(s => s.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

            ListingSale? capturedListingSale = null;
            _listingSaleWriteServiceMock.Setup(s => s.AddListingSaleAsync(It.IsAny<ListingSale>())).Callback<ListingSale>(c => capturedListingSale = c);

            // Act
            ListingSaleInfo actualSaleInfoFromUseCase = await _approveReservationSellUseCase.ExecuteAsync(reservation.Id, reservation.SellerUserId);

            // Assert
            capturedListingSale.Should().NotBeNull();
            capturedListingSale.ReservationId.Should().Be(reservation.Id);
            capturedListingSale.ListingId.Should().Be(listing.Id);
            actualSaleInfoFromUseCase.Should().BeEquivalentTo(capturedListingSale, options => options.ExcludingMissingMembers());

            _listingSaleWriteServiceMock.Verify(s => s.AddListingSaleAsync(It.IsAny<ListingSale>()), Times.Once);
        }
    }
}
