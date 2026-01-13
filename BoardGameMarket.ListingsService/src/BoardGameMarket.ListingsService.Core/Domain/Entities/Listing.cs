using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	/// <summary>
	/// Represents a single sale, single entry or bundle (seller side)
	/// </summary>
	public class Listing
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public Guid ListingGroupId { get; set; }
		public ListingStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
