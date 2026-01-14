using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Filters
{
	public class ListingGroupFilter
	{
		public Guid? SellerUserId {  get; set; }
		public Guid? CatalogId { get; set; }
		public string? Location { get; set; }
		public IEnumerable<Guid>? ListingGroupIds { get; set; }
	}
}
