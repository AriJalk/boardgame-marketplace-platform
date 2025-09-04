namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	/// <summary>
	/// Represents a single game that is part of a listing
	/// </summary>
	public class ListingGame
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public Guid GameId { get; set; }
	}
}
