using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
    public static class QueueDomainService
    {
        public const int INITIAL_PRIORITY = 0;
        public const int PRIORITY_GAP = 1000;

        private static IEnumerable<ListingQueueEntry> SortQueue(IEnumerable<ListingQueueEntry> queue)
        {
            return queue.OrderBy(qe => qe.Priority.HasValue ? qe.Priority : int.MaxValue).ThenBy(qe => qe.CreatedAt);
        }

        public static void ValidateReserve(IEnumerable<ListingQueueEntry> queue, ListingQueueEntry requestedQueueEntry)
        {
            List<string> errors = new List<string>();
            IEnumerable<ListingQueueEntry> sortedQueue = SortQueue(queue);
            ListingQueueEntry? headQueueEntry = sortedQueue.FirstOrDefault();
            if (headQueueEntry == null)
            {
                throw new KeyNotFoundException("Queue is empty");
            }
            if (headQueueEntry.Id != requestedQueueEntry.Id)
            {
                throw new InvalidOperationException("Requested queue entry not at top priority");
            }
            if (headQueueEntry.Status != QueueStatus.Queued)
            {
                throw new InvalidOperationException("Requesed queue entry not at Queued status");
            }
        }

        /// <summary>
        /// Normalize priorities of a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <returns>A new collection of the normalized queue</returns>
        public static IEnumerable<ListingQueueEntry> Normalize(this IEnumerable<ListingQueueEntry> queue)
        {
            List<ListingQueueEntry> queueList = queue.ToList();
            List<ListingQueueEntry> normalizedQueue = new List<ListingQueueEntry>();
            for (int i = 0; i < queue.Count(); i++)
            {
                normalizedQueue.Add(new ListingQueueEntry(queueList[i], i * PRIORITY_GAP, DateTime.UtcNow));
            }
            return normalizedQueue;
        }
    }
}