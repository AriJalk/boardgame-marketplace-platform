using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups
{
	public record ListingGroupAddRequest(Guid SellerUserId, string? Details, string PickupOptions, string Location) { }
}
