namespace TestingService.Dtos
{
	public record ListingGroupDetailedDto(Guid Id, UserSummaryDto Seller, DateTime PostingTime, string PickupOptions, string Location, string? Details, IEnumerable<ListingDetailDto> Listings) { }
}
