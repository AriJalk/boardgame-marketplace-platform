using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private IListingReadService _listingReadService;
		private ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

		public ListingsController(IListingReadService listingReadService, ListingQueueAggregateBuilder listingQueueAggregateBuilder)
		{
			_listingReadService = listingReadService;
			_listingQueueAggregateBuilder = listingQueueAggregateBuilder;
		}
	}
}
