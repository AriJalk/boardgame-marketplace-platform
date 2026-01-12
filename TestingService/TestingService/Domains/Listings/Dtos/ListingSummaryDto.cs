namespace TestingService.Domains.Listings.Dtos
{
	public class ListingSummaryDto
	{
		public Guid Id { get; set; }
		public string? Title { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
