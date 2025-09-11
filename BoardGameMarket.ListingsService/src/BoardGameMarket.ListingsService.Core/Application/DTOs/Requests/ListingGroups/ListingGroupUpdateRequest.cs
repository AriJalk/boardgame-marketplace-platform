using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups
{
	internal class ListingGroupUpdateRequest
	{
		public Guid Id { get; set; }
		public string? Details { get; set; }
		public string PickupOptions { get; set; }
		public string Location { get; set; }
	}
}
