using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries
{
	internal class ListingQueueEntrySetPriorityRequest
	{
		public Guid ListingQueueEntryId { get; set; }
		public decimal Priority { get; set; }
	}
}
