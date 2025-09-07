using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries
{
	public class ListingQueueEntryUpdateRequest
	{
		public Guid Id { get; set; }
		public Guid ListingId { get; set; }
		public QueueStatus Status { get; set; }
		public Guid BuyerUserId { get; set; }
	}
}
