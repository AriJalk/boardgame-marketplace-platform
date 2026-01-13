
namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	/// <summary>
	/// Represents a collection of listings
	/// </summary>
	public class ListingGroup
	{
		public Guid Id { get; set; }
		public Guid SellerUserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public string? Details { get; set; }
		public string PickupOptions { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
	}
}
