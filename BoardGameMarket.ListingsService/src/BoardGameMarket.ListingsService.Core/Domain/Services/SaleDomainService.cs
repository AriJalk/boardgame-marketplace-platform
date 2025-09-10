using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class SaleDomainService
	{
		public Result CanSell(ListingQueueEntry requestQueueEntry, Listing listing)
		{
			if (listing.Status != ListingStatus.Pending)
			{
				return Result.Failure("Listing is not in pending status");
			}
			if (requestQueueEntry.Status != QueueStatus.Approved)
			{
				return Result.Failure("Queue entry is not approved");
			}

			return Result.Success();
		}
	}
}
