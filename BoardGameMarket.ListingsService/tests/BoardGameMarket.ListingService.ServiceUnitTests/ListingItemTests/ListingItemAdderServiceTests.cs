using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameMarket.ListingService.ServiceUnitTests.ListingItemTests
{
	public class ListingItemAdderServiceTests
	{
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly IFixture _fixture;
		private readonly Mock<ILogger<ListingItemAdderService>> _loggerMock;
		private readonly Mock<IListingItemRepository> _listingItemRepositoryMock;
		public ListingItemAdderServiceTests()
		{
			_fixture = new Fixture();
			_loggerMock = new Mock<ILogger<ListingItemAdderService>>();
			_listingItemRepositoryMock = new Mock<IListingItemRepository>();
			_listingItemAdderService = new ListingItemAdderService(_loggerMock.Object, _listingItemRepositoryMock.Object);
		}


		[Fact]
		public async Task AddListingItem_NullAddRequest_ThrowsArgumentNullException()
		{
			// Arrange
			ListingItemAddRequest? addRequest = null;

			// Act
			Func<Task> task = async () =>
			{
				await _listingItemAdderService.AddListingItemAsync(addRequest);
			};

			// Assert
			await task.Should().ThrowAsync<ArgumentNullException>();
		}

		//[Fact]
		//public async Task AddListingItem_InvalidAddRequest_ThrowsArgumentException()
		//{
		//	// Arrange
		//	ListingItemAddRequest addRequest = _fixture.Create<ListingItemAddRequest>();

		//	// Act
		//	Func<Task> func = async () =>
		//	{
		//		await _listingItemAdderService.AddListingItemAsync(addRequest);
		//	};

		//	// Assert
		//	await func.Should().ThrowAsync<ArgumentException>();
		//}

		[Fact]
		public async Task AddListingItem_ValidAddRequest_ReturnsListingItem()
		{
			// Arrange
			ListingItemAddRequest addRequest = _fixture.Create<ListingItemAddRequest>();

			ListingItem expected = new ListingItem()
			{
				Id = Guid.NewGuid(),
				BggItemId = addRequest.BggItemId,
				ListingId = addRequest.ListingId,
			};

			_listingItemRepositoryMock.Setup(r => r.AddListingItemAsync(It.IsAny<ListingItem>())).ReturnsAsync(expected);

			// Act

			ListingItem? listingItem = await _listingItemAdderService.AddListingItemAsync(addRequest);

			// Assert
			listingItem.Should().NotBeNull();
			listingItem.Id = expected.Id;

			listingItem.Should().BeEquivalentTo(expected);
		}

	}
}
