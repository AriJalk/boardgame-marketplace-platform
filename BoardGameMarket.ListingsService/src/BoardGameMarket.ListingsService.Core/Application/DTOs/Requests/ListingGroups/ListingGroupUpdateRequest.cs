namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups
{
	internal class ListingGroupUpdateRequest
	{
		public string? Details { get; set; }
		public string PickupOptions { get; set; }
		public string Location { get; set; }
	}
}
