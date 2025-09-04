using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingGames;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingService.ServiceUnitTests.ListingGameTests
{
	public class ListingGameAdderServiceTests
	{
		private readonly IListingGameAdderService _listingGameAdderService;
		private readonly IFixture _fixture;
		private readonly Mock<ILogger<ListingGameAdderService>> _loggerMock;
		private readonly Mock<IListingGameRepository> _listingGameRepositoryMock;
		public ListingGameAdderServiceTests()
		{
			_fixture = new Fixture();
			_loggerMock = new Mock<ILogger<ListingGameAdderService>>();
			_listingGameRepositoryMock = new Mock<IListingGameRepository>();
			_listingGameAdderService = new ListingGameAdderService(_loggerMock.Object, _listingGameRepositoryMock.Object);
		}


		[Fact]
		public async Task AddListingGame_NullAddRequest_ReturnsNull()
		{
			// Arrange
			ListingGameAddRequest? addRequest = null;

			// Act

			ListingGame? response = await _listingGameAdderService.AddListingGame(addRequest);


			// Assert
			response.Should().BeNull();
		}

		//[Fact]
		//public async Task AddListingGame_InvalidAddRequest_ThrowsArgumentException()
		//{
		//	// Arrange
		//	ListingGameAddRequest addRequest = _fixture.Create<ListingGameAddRequest>();

		//	// Act
		//	Func<Task> func = async () =>
		//	{
		//		await _listingGameAdderService.AddListingGame(addRequest);
		//	};

		//	// Assert
		//	await func.Should().ThrowAsync<ArgumentException>();
		//}

		[Fact]
		public async Task AddListingGame_ValidAddRequest_ReturnsListingGame()
		{
			// Arrange
			ListingGameAddRequest addRequest = _fixture.Create<ListingGameAddRequest>();

			ListingGame expected = new ListingGame()
			{
				Id = Guid.NewGuid(),
				GameId = addRequest.GameId,
				ListingId = addRequest.ListingId,
			};

			_listingGameRepositoryMock.Setup(r => r.AddListingGameAsync(It.IsAny<ListingGame>())).ReturnsAsync(expected);

			// Act

			ListingGame? listingGame = await _listingGameAdderService.AddListingGame(addRequest);

			// Assert
			listingGame.Should().NotBeNull();
			listingGame.Id = expected.Id;

			listingGame.Should().BeEquivalentTo(expected);
		}

	}
}
