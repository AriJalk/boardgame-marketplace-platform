namespace TestingService.Dtos
{
	public record CatalogItemDto(Guid Id, string Name, string? Type, string? BggLink, string? ImageUrl) { }
}
