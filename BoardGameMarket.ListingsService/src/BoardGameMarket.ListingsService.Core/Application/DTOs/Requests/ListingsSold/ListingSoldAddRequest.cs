using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold
{
	public class ListingSoldAddRequest
	{
		public Guid ListingId { get; set; }
		public Guid ListingQueueEntryId { get; set; }
		public DateTime CompletedAt { get; set; }
		public Guid SellerId { get; set; }
		public Guid BuyerId { get; set; }
	}
}
