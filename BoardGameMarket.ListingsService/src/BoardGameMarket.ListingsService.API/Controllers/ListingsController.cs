using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.AggregateServices;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
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

		[HttpGet("{listingId:guid}/queues")]
		public async Task<ActionResult<ListingQueueInfo>> GetQueue(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				return NotFound();
			}
			ListingQueueAggregate listingQueue = await _listingQueueAggregateService.GetListingQueueAggregate(listingId);

			ListingQueueEntryInfo[] queueByTime = listingQueue.QueueByTime.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();
			ListingQueueEntryInfo[] queueByPriority = listingQueue.QueueByPriority.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();

			return Ok(new ListingQueueInfo(queueByTime, queueByPriority));
		}
	}
}
