using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingSoldMappers
	{
		public static ListingSold ToListingSold(this ListingSoldAddRequest listingSoldAddRequest)
		{
			return new ListingSold()
			{
				ListingId = listingSoldAddRequest.ListingId,
				ListingQueueEntryId = listingSoldAddRequest.ListingQueueEntryId,
				SellerId = listingSoldAddRequest.SellerId,
				BuyerId = listingSoldAddRequest.BuyerId,
			};
		}
	}
}
