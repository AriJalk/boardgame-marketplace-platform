using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
    /// <summary>
    /// A reservation queue entry (buyer side)
    /// </summary>
    public class ListingQueueEntry
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public QueueStatus Status { get; set; }
        public Guid BuyerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public decimal? Priority { get; set; }

        public ListingQueueEntry() { }

        public ListingQueueEntry(ListingQueueEntry other, int? newPriority, DateTime? modifiedAt = null)
        {
            Id = other.Id;
            ListingId = other.ListingId;
            Status = other.Status;
            BuyerUserId = other.BuyerUserId;
            CreatedAt = other.CreatedAt;
            ModifiedAt = modifiedAt ?? other.ModifiedAt;

            Priority = newPriority ?? other.Priority;
        }
    }
}
