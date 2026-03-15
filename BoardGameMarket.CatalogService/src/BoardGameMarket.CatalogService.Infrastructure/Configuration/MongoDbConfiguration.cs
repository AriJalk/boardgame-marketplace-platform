namespace BoardGameMarket.CatalogService.Infrastructure.Configuration
{
	internal class MongoDbConfiguration
	{
		public string? MongoHost { get; set; }
		public string? MongoPort { get; set; }
		public string? MongoDatabase { get; set; }
		public string? CollectionName { get; set; }
	}
}
