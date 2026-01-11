using TestingService.Dtos.Item;

namespace TestingService.Dtos.Listing
{
	public class ListingSummaryDto
	{
		public Guid Id { get; set; }
		public string? Title { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
