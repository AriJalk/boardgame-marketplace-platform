using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/listings/{listingId:guid}/[controller]")]
	[ApiController]
	public class QueueController : ControllerBase
	{
		private readonly GetListingQueueUseCase _getListingQueueUseCase;

		public QueueController(GetListingQueueUseCase getListingQueueUseCase)
		{
			_getListingQueueUseCase = getListingQueueUseCase;
		}

		[HttpGet()]
		public async Task<ActionResult<ListingQueueResponseDto>> GetQueue(Guid listingId)
		{
			ListingQueueResponseDto response = await _getListingQueueUseCase.ExecuteAsync(listingId);

			return Ok(response);
		}
	}
}
