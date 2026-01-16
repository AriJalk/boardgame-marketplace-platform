using BoardGameMarket.CatalogService.Core.Domain.Enums;

namespace BoardGameMarket.CatalogService.Core.Domain.Entities
{
	public class CatalogItem
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public CatalogItemType Type { get; set; }
		public int? BggId { get; set; }
		public string? ImageUrl { get; set; }
		public IReadOnlyCollection<string>? Categories { get; set; }
		public DateTime ModifiedTimestamp { get; set; }
	}
}
