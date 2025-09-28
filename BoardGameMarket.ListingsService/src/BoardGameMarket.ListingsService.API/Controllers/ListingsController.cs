using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.UseCases.Listings;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly AddCompleteListingsUseCase _addCompleteListingsUseCase;
		private readonly DeleteListingUseCase _deleteListingUseCase;
		private readonly UpdateListingUseCase _updateListingUseCase;

		public ListingsController(AddCompleteListingsUseCase addCompleteListingsUseCase, DeleteListingUseCase deleteListingUseCase, UpdateListingUseCase updateListingUseCase)
		{
			_addCompleteListingsUseCase = addCompleteListingsUseCase;
			_deleteListingUseCase = deleteListingUseCase;
			_updateListingUseCase = updateListingUseCase;
		}

		[HttpPost]
		public async Task<ActionResult<ListingResponseDto[]>> AddCompleteListings(IEnumerable<ListingWithListingItemsRequest> listingWithListingItemsRequests)
		{

			IEnumerable<ListingResponseDto> responses = await _addCompleteListingsUseCase.ExecuteAsync(listingWithListingItemsRequests);

			return Ok(responses);
		}

		[HttpDelete("{listingId:guid}")]
		public async Task<IActionResult> DeleteListing(Guid listingId)
		{
			bool isDeleted = await _deleteListingUseCase.ExecuteAsync(listingId);
			if (isDeleted)
			{
				return NoContent();
			}
			return NotFound();
		}

		[HttpPut("{listingId:guid}")]
		public async Task<ActionResult<ListingResponseDto>> UpdateListing(ListingUpdateRequest updateRequest, Guid listindId)
		{
			ListingResponseDto response = await _updateListingUseCase.ExecuteAsync(updateRequest, listindId);
			return Ok(response);
		}
	}
}
