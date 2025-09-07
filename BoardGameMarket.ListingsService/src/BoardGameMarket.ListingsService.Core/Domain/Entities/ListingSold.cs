namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	public class ListingSold
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public Guid ListingQueueEntryId { get; set; }
		public DateTime CompletedAt { get; set; }
		public Guid SellerId { get; set; } // Duplication for fast lookup
		public Guid BuyerId { get; set; } // Duplication for fast lookup
	}
}
