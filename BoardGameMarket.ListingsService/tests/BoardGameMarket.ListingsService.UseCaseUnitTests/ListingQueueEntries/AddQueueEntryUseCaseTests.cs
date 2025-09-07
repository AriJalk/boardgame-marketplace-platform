using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueueEntries;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameMarket.ListingsService.UseCaseUnitTests.ListingQueueEntries
{
	public class AddQueueEntryUseCaseTests
	{
		private readonly AddQueueEntryUseCase _addQueueEntryUseCase;
		private readonly Mock<IListingQueueEntryGetterService> _listingQueueEntryGetterServiceMock;
		private readonly Mock<IListingQueueEntryAdderService> _listingQueueEntryAdderServiceMock;
		private readonly Mock<IListingGetterService> _listingGetterServiceMock;
		private readonly Mock<IListingGroupGetterService> _listingGroupGetterServiceMock;
		private readonly Mock<ILogger<AddQueueEntryUseCase>> _loggerMock;

		public AddQueueEntryUseCaseTests()
		{
			_listingQueueEntryGetterServiceMock = new Mock<IListingQueueEntryGetterService>();
			_listingQueueEntryAdderServiceMock = new Mock<IListingQueueEntryAdderService>();
			_listingGetterServiceMock = new Mock<IListingGetterService>();
			_listingGroupGetterServiceMock = new Mock<IListingGroupGetterService>();
			_loggerMock = new Mock<ILogger<AddQueueEntryUseCase>>();
			_addQueueEntryUseCase = new AddQueueEntryUseCase(_listingQueueEntryGetterServiceMock.Object, _listingQueueEntryAdderServiceMock.Object, _listingGetterServiceMock.Object, _listingGroupGetterServiceMock.Object, _loggerMock.Object);
		}
	}
}
