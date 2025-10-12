using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingItemsController : ControllerBase
	{

		private readonly AddListingItemUseCase _addListingItemUseCase;
		private readonly DeleteListingItemUseCase _deleteListingItemUseCase;

		public ListingItemsController(AddListingItemUseCase addListingItemUseCase, DeleteListingItemUseCase deleteListingItemUseCase)
		{
			_addListingItemUseCase = addListingItemUseCase;
			_deleteListingItemUseCase = deleteListingItemUseCase;
		}

		[HttpPost]
		public async Task<ActionResult<ListingItemResponseDto>> AddListingItem(ListingItemAddRequest listingItemAddRequest)
		{
			ListingItemResponseDto listingItemResponseDto = await _addListingItemUseCase.ExecuteAsync(listingItemAddRequest);
			return Ok(listingItemResponseDto);
		}

		[HttpDelete("{listingItemId:guid}")]
		public async Task<IActionResult> DeleteListingItem(Guid listingItemId)
		{
			bool isDeleted = await _deleteListingItemUseCase.Execute(listingItemId);
			if (isDeleted)
			{
				return NoContent();
			}
			return NotFound();
		}
	}
}
