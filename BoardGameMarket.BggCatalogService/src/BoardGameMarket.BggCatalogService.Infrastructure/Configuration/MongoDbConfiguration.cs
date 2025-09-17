namespace BoardGameMarket.BggCatalogService.Infrastructure.Configuration
{
	internal class MongoDbConfiguration
	{
		public string? MongoHost { get; set; }
		public string? MongoPort { get; set; }
		public string? DatabaseName { get; set; }
		public string? CollectionName { get; set; }
	}
}
