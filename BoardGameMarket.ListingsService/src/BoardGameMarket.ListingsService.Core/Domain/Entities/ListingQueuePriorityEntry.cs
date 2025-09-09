using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
	public class ListingQueuePriorityEntry
	{
		public Guid ListingQueueEntryId { get; set; }
		public decimal Priority { get; set; }
	}
}
