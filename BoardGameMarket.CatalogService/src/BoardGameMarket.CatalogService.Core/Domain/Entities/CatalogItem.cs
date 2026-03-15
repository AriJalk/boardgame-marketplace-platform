using BoardGameMarket.CatalogService.Core.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace BoardGameMarket.CatalogService.Core.Domain.Entities
{
	public class CatalogItem
	{
		[BsonId]
		public Guid Id { get; set; }
		[BsonElement("name")]
		public string Name { get; set; } = string.Empty;
		[BsonElement("type")]
		public CatalogItemType Type { get; set; }
		[BsonElement("bggId")]
		public int? BggId { get; set; }
		[BsonElement("imageUrl")]
		public string? ImageUrl { get; set; }
		[BsonElement("categories")]
		public IReadOnlyCollection<string>? Categories { get; set; }
		[BsonElement("modifiedTimestamp")]
		public DateTime ModifiedTimestamp { get; set; }
	}
}
