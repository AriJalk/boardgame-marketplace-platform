using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.AggregateServices;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private IListingGetterService _listingGetterService;
		private ListingQueueAggregateService _listingQueueAggregateService;

		public ListingsController(IListingGetterService listingGetterService, ListingQueueAggregateService listingQueueAggregateService)
		{
			_listingGetterService = listingGetterService;
			_listingQueueAggregateService = listingQueueAggregateService;
		}
	}
}
