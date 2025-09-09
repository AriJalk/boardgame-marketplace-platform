using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class SaleDomainService
	{
		private bool IsPrioritySaleValid(ListingQueueEntry listingQueueEntry, ListingQueuePriorityEntry topPriorityEntry)
		{
			return listingQueueEntry.Id == topPriorityEntry.ListingQueueEntryId && listingQueueEntry.Status == Enums.QueueStatus.Pending;
		}
		private bool IsRegularSaleValid(ListingQueueEntry listingQueueEntry, ListingQueueEntry topListingQueueEntry)
		{
			return listingQueueEntry.Id == topListingQueueEntry.Id && listingQueueEntry.Status == Enums.QueueStatus.Pending;
		}

		public Result CanSell(ListingQueueEntry requestQueueEntry, Queue<ListingQueueEntry> listingQueueEntries, Queue<ListingQueuePriorityEntry> listingQueuePriorityEntries)
		{
			// Priority queue first
			if (listingQueuePriorityEntries.TryPeek(out ListingQueuePriorityEntry? headPriorityEntry))
			{
				if (!IsPrioritySaleValid(requestQueueEntry, headPriorityEntry))
				{
					return Result.Failure("QueueEntry not matching Head of priority list");
				}
			}
			else if (listingQueueEntries.TryPeek(out ListingQueueEntry? headEntry))
			{
				if (!IsRegularSaleValid(requestQueueEntry, headEntry))
				{
					return Result.Failure("QueueEntry not matching head of queue");
				}
			}

			return Result.Success();
		}
	}
}
