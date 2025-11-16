namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	/// <summary>
	/// Represents a single item that is part of a listing
	/// </summary>
	public class ListingItem
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public Guid CatalogId { get; set; }
		public string? Details { get; set; }
	}
}
