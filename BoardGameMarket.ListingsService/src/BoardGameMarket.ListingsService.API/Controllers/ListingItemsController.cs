using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingItemsController : ControllerBase
	{

		private readonly AddListingItemUseCase addListingItemUseCase;

		public ListingItemsController(AddListingItemUseCase addListingItemUseCase)
		{
			this.addListingItemUseCase = addListingItemUseCase;
		}

		[HttpPost]
		public async Task<ActionResult<ListingItemResponseDto>> AddListingItem(ListingItemAddRequest listingItemAddRequest)
		{
			ListingItemResponseDto listingItemResponseDto = await addListingItemUseCase.ExecuteAsync(listingItemAddRequest);
			return Ok(listingItemResponseDto);
		}
	}
}
