namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	public class ListingSale
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public DateTime CompletedAt { get; set; }
		public Guid ReservationId { get; set; }
		public Guid ListingQueueEntryId { get; set; } // Duplication
		public Guid SellerUserId { get; set; } // Duplication for fast lookup
		public Guid BuyerUserId { get; set; } // Duplication for fast lookup
	}
}
