using AutoFixture;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.UnitTests.UseCases.ListingQueueUseCases
{
    public class GetListingQueueEntryTestsBase
    {
        protected readonly Mock<IListingQueueEntryReadService> _listingQueueEntryReadServiceMock;
        protected readonly Mock<IListingReadService> _listingReadServiceMock;

        protected readonly IFixture _fixture;

        public GetListingQueueEntryTestsBase()
        {
            _listingQueueEntryReadServiceMock = new Mock<IListingQueueEntryReadService>();
            _listingReadServiceMock = new Mock<IListingReadService>();

            _fixture = new Fixture();
        }
    }
}
