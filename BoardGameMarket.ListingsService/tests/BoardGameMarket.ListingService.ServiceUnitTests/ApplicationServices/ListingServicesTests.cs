using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Moq;

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
				_listingRepositoryMock.Verify(r => r.AddListingAsync(listingToAdd), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.AddListingAsync(listingToAdd), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.AddListingsAsync(listingsToAdd), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.AddListingsAsync(listingsToAdd), Times.Once);
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
				_listingRepositoryMock.Setup(r => r.DeleteListingAsync(It.IsAny<Guid>())).ReturnsAsync(true);
				// Act
				bool result = await _listingDeleterService.DeleteListingAsync(listingId);

				// Assert
				result.Should().BeTrue();
				_listingRepositoryMock.Verify(r => r.DeleteListingAsync(listingId), Times.Once);
			}

			[Fact]
			public async Task DeleteListingAsync_InvalidId_ReturnsFalse()
			{
				// Arrange
				Guid listingId = Guid.NewGuid();
				_listingRepositoryMock.Setup(r => r.DeleteListingAsync(It.IsAny<Guid>())).ReturnsAsync(false);

				// Act
				bool result = await _listingDeleterService.DeleteListingAsync(listingId);

				// Assert
				result.Should().BeFalse();
				_listingRepositoryMock.Verify(r => r.DeleteListingAsync(listingId), Times.Once);
			}

			[Fact]
			public async Task DeleteListingsByListingGroupId_CallsRepoOnce()
			{
				// Arrange 
				Guid groupId = Guid.NewGuid();

				// Act
				await _listingDeleterService.DeleteListingsByListingGroupId(groupId);

				// Assert
				_listingRepositoryMock.Verify(r => r.DeleteListingsByListingGroupIdAsync(groupId), Times.Once);
			}
		}

		public class ListingGetterServiceTests : ListingServicesTests
		{
			private readonly IListingGetterService _listingGetterService;

			public ListingGetterServiceTests()
			{
				_listingGetterService = new ListingGetterService(_listingRepositoryMock.Object);
			}

			[Fact]
			public async Task GetListingByIdAsync_ListingExists_ReturnsListing()
			{
				// Arrange
				Listing listingToFetch = _fixture.Create<Listing>();
				_listingRepositoryMock.Setup(r => r.GetListingByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listingToFetch);

				// Act
				Listing? listingFromService = await _listingGetterService.GetListingByIdAsync(listingToFetch.Id);

				// Assert
				listingFromService.Should().NotBeNull();
				listingFromService.Should().BeEquivalentTo(listingToFetch);
				_listingRepositoryMock.Verify(r => r.GetListingByIdAsync(listingToFetch.Id), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.GetListingByIdAsync(listingId), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.GetListingsByGroupIdAsync(groupId), Times.Once);
			}

			[Fact]
			public async Task GetListingsByListingGroupIdAsync_ValidGroup_ReturnsCollection()
			{
				// Arrange
				Guid groupId = Guid.NewGuid();
				IEnumerable<Listing> listingsToFetch = _fixture.Build<Listing>()
					.With(l => l.ListingGroupId, groupId)
					.CreateMany(5);

				_listingRepositoryMock.Setup(r => r.GetListingsByGroupIdAsync(It.IsAny<Guid>())).ReturnsAsync(listingsToFetch);

				// Act
				IEnumerable<Listing> listingsFromService = await _listingGetterService.GetListingsByListingGroupIdAsync(groupId);

				// Assert
				listingsFromService.Should().BeEquivalentTo(listingsToFetch);
				_listingRepositoryMock.Verify(r => r.GetListingsByGroupIdAsync(groupId), Times.Once);
			}
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
				_listingRepositoryMock.Verify(r => r.UpdateListingAsync(listingToUpdate), Times.Once);
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
				_listingRepositoryMock.Verify(r => r.UpdateListingAsync(listingToUpdate), Times.Once);
			}
		}
	}
}
