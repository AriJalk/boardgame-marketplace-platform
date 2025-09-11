using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Responses
{
	internal class ListingResponseDto
	{
		public Guid Id { get; set; }
		public ListingStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
		public IEnumerable<ListingItemResponseDto>? ListingItems { get; set; }
		public IEnumerable<ListingQueueEntryResponseDto>? QueueEntries { get; set; }
		public Guid? ListingGroupId { get; set; } // Optional
	}
}
