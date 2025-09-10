using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	internal class ListingSoldAddRequestComposer
	{
		private readonly ListingFetcher _listingFetcher;
		public async Task<ListingSoldAddRequest> ComposeListingSoldAddRequest(Guid listingId, Guid listingQueueEntryId)
		{
			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingId(listingId);
			Guid buyerId = (await _listingFetcher.GetListingQueueEntryAsync(listingQueueEntryId)).BuyerUserId;
			return new ListingSoldAddRequest()
			{
				ListingId = listingId,
				ListingQueueEntryId = listingQueueEntryId,
				SellerId = sellerId,
				BuyerId = buyerId,
			};

		}
	}
}
