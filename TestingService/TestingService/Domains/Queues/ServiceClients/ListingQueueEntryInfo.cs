using TestingService.Domains.Queues.Enums;

namespace TestingService.Domains.Queues.ServiceClients
{
	public class ListingQueueEntryInfo
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public Guid BuyerUserId { get; set; }
		public QueueStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public decimal? Priority { get; set; }
	}
}
