namespace TestingService.Dtos.Item
{
	public class CatalogItemDto
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Type { get; set; }
		public string? BggLink { get; set; }
		public string? ImageUrl { get; set; }
	}
}
