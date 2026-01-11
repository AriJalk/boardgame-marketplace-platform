using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Responses
{
	public class ListingResponseDto
	{
		public Guid Id { get; set; }
		public string? Title { get; set; }
		public ListingStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
		public ListingItemResponseDto[]? ListingItems { get; set; }
		// TODO: remove from dto, must be seperate endpoint
		public ListingQueueEntryResponseDto[]? QueueEntries { get; set; }
		public Guid? ListingGroupId { get; set; } // Optional
	}
}
