using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Services.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingService.ServiceUnitTests.ApplicationServices
{
	public class ListingServicesTests
	{
		protected readonly IFixture _fixture;
		protected readonly Mock<IListingRepository> _listingRepositoryMock;

		public ListingServicesTests()
		{
			_fixture = new Fixture();
			_listingRepositoryMock = new Mock<IListingRepository>();
		}

		public class ListingAdderServiceTests : ListingServicesTests
		{
			private readonly IListingAdderService _listingAdderService;

			public ListingAdderServiceTests()
			{
				_listingAdderService = new ListingAdderService(_listingRepositoryMock.Object);
			}

			[Fact]
			public async Task AddListingAsync_ValidListing_ReturnsSameListing()
			{
				// Arrange
				Listing listingToAdd = _fixture.Create<Listing>();

				_listingRepositoryMock.Setup(r => r.AddListingAsync(It.IsAny<Listing>())).ReturnsAsync(listingToAdd);

				// Act
				Listing? listingFromService = await _listingAdderService.AddListingAsync(listingToAdd);

				// Assert
				listingFromService.Should().NotBeNull();
				listingFromService.Should().BeEquivalentTo(listingToAdd);
			}

			[Fact]
			public async Task AddListingAsync_InvalidListing_ReturnsNull()
			{
				// Arrange
				Listing listingToAdd = _fixture.Create<Listing>();

				_listingRepositoryMock.Setup(r => r.AddListingAsync(It.IsAny<Listing>())).ReturnsAsync(null as Listing);

				// Act
				Listing? listingFromService = await _listingAdderService.AddListingAsync(listingToAdd);

				// Assert
				listingFromService.Should().BeNull();
			}

			[Fact]
			public async Task AddListingsAsync_ValidListings_ReturnsListings()
			{
				// Arrange
				IEnumerable<Listing> listingsToAdd = _fixture.Create<IEnumerable<Listing>>();

				_listingRepositoryMock.Setup(r => r.AddListingsAsync(It.IsAny<IEnumerable<Listing>>())).ReturnsAsync(listingsToAdd);

				// Act
				IEnumerable<Listing> listingsFromService = await _listingAdderService.AddListingsAsync(listingsToAdd);

				// Assert
				listingsFromService.Should().BeEquivalentTo(listingsToAdd);
			}

			[Fact]
			public async Task AddListingsAsync_EmptyCollection_ReturnsEmptyCollection()
			{
				// Arrange
				IEnumerable<Listing> listingsToAdd = Enumerable.Empty<Listing>();

				_listingRepositoryMock.Setup(r => r.AddListingsAsync(It.IsAny<IEnumerable<Listing>>())).ReturnsAsync(Array.Empty<Listing>());

				// Act
				IEnumerable<Listing> listingsFromService = await _listingAdderService.AddListingsAsync(listingsToAdd);

				// Assert
				listingsFromService.Should().BeNullOrEmpty();
			}
		}

		public class ListingDeleterServiceTests : ListingServicesTests
		{
			private readonly IListingDeleterService _listingDeleterService;

			public ListingDeleterServiceTests()
			{
				_listingDeleterService = new ListingDeleterService(_listingRepositoryMock.Object);
			}

			[Fact]
			public async Task DeleteListingAsync_ValidId_ReturnsTrue()
			{
				// Arrange
				Guid listingId = Guid.NewGuid();
				_listingRepositoryMock.Setup(r => r.DeleteListingAsync(It.Is<Guid>(id => id == listingId))).ReturnsAsync(true);
				// Assert
				bool result = await _listingDeleterService.DeleteListingAsync(listingId);
				result.Should().BeTrue();
			}

			[Fact]
			public async Task DeleteListingAsync_InvalidId_ReturnsFalse()
			{
				// Arrange
				Guid listingId = Guid.NewGuid();
				_listingRepositoryMock.Setup(r => r.DeleteListingAsync(It.Is<Guid>(id => id == listingId))).ReturnsAsync(false);
				// Act
				bool result = await _listingDeleterService.DeleteListingAsync(listingId);

				// Assert
				result.Should().BeFalse();
			}
		}

		public class ListingGetterServiceTests : ListingServicesTests
		{
			private readonly IListingGetterService _listingGetterService;

			public ListingGetterServiceTests()
			{
				_listingGetterService = new ListingGetterService(_listingRepositoryMock.Object, new RequestCache());
			}

			[Fact]
			public async Task GetListingByIdAsync_ListingExists_ReturnsListing()
			{
				// Arrange
				Listing listingToFetch = _fixture.Create<Listing>();
				_listingRepositoryMock.Setup(r => r.GetListingByIdAsync(It.Is<Guid>(id => id == listingToFetch.Id))).ReturnsAsync(listingToFetch);

				// Act
				Listing? listingFromService = await _listingGetterService.GetListingByIdAsync(listingToFetch.Id);

				// Assert
				listingFromService.Should().NotBeNull();
				listingFromService.Should().BeEquivalentTo(listingToFetch);
			}

			[Fact]
			public async Task GetListingByIdAsync_ListingNotExists_ReturnsNull()
			{
				// Arrange
				Guid listingId = Guid.NewGuid();
				_listingRepositoryMock.Setup(r => r.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Listing);

				// Act
				Listing? listingFromService = await _listingGetterService.GetListingByIdAsync(listingId);

				// Assert
				listingFromService.Should().BeNull();
			}

			[Fact]
			public async Task GetListingsByListingGroupIdAsync_InvalidGroup_ReturnsEmptyCollection()
			{
				// Arrange
				Guid groupId = Guid.NewGuid();
				_listingRepositoryMock.Setup(r => r.GetListingsByGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync(Enumerable.Empty<Listing>());

				// Act
				IEnumerable<Listing> listingsFromService = await _listingGetterService.GetListingsByListingGroupIdAsync(groupId);

				// Assert
				listingsFromService.Should().BeNullOrEmpty();
			}

			[Fact]
			public async Task GetListingsByListingGroupIdAsync_ValidGroup_ReturnsCollection()
			{
				// Arrange
				Guid groupId = Guid.NewGuid();
				IEnumerable<Listing> listingsToFetch = _fixture.Build<Listing>()
					.With(l => l.ListingGroupId, groupId)
					.CreateMany(5);

				_listingRepositoryMock.Setup(r => r.GetListingsByGroupIdAsync(It.Is<Guid>(id => id == groupId))).ReturnsAsync(listingsToFetch);

				// Act
				IEnumerable<Listing> listingsFromService = await _listingGetterService.GetListingsByListingGroupIdAsync(groupId);

				// Assert
				listingsFromService.Should().BeEquivalentTo(listingsToFetch);
			}

			public class ListingUpdaterServiceTests : ListingServicesTests
			{
				private readonly IListingUpdaterService _listingUpdaterService;

				public ListingUpdaterServiceTests()
				{
					_listingUpdaterService = new ListingUpdaterService(_listingRepositoryMock.Object);
				}

				[Fact]
				public async Task UpdateListingAsync_InvalidUpdateRequest_ReturnsNull()
				{
					// Arrange
					Listing listingToUpdate = _fixture.Create<Listing>();

					_listingRepositoryMock.Setup(r => r.UpdateListingAsync(It.IsAny<Listing>())).ReturnsAsync(null as Listing);
					// Act
					Listing? listingFromService = await _listingUpdaterService.UpdateListingAsync(listingToUpdate);

					// Assert
					listingFromService.Should().BeNull();
				}

				[Fact]
				public async Task UpdateListingAsync_ValidUpdateRequest_ReturnsListing()
				{
					// Arrange
					Listing listingToUpdate = _fixture.Create<Listing>();

					_listingRepositoryMock.Setup(r => r.UpdateListingAsync(It.IsAny<Listing>())).ReturnsAsync(listingToUpdate);
					// Act
					Listing? listingFromService = await _listingUpdaterService.UpdateListingAsync(listingToUpdate);

					// Assert
					listingFromService.Should().NotBeNull();

					listingToUpdate.ModifiedAt = listingFromService.ModifiedAt;
					listingFromService.Should().BeEquivalentTo(listingToUpdate);
				}
			}
		}
	}
}
