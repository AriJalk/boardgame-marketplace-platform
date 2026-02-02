using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters
{
	public class ListingGroupFilter
	{
		public Guid? SellerUserId {  get; set; }
		public Guid? CatalogId { get; set; }
		public string? Location { get; set; }
		public Guid[]? ListingGroupIds { get; set; }
		public Guid[]? ListingIds { get; set; }
	}
}
