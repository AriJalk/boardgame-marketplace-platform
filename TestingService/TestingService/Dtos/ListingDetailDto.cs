namespace TestingService.Dtos
{
	public record ListingDetailDto(Guid Id, string Title, decimal Price, string? Details, IEnumerable<ListingItemDto> Items) { }
}
