using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups
{
	public class ListingGroupUpdateRequest
	{
		public Guid Id { get; set; }
		public Guid SellerUserId { get; set; }
		public string? Details { get; set; }
		public string PickupOptions { get; set; }
		public string Location { get; set; }
	}
}
