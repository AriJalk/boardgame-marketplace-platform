namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	/// <summary>
	/// Represents a single item that is part of a listing
	/// </summary>
	public class ListingItem
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public Guid BggItemId { get; set; }

		//TODO: add name/details to override item service data
	}
}
