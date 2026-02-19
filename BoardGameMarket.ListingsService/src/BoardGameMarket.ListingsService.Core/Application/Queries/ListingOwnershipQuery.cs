using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Queries
{
	public class ListingOwnershipQuery
	{
		private readonly IListingReadService _listingReadService;
		private readonly IListingGroupReadService _listingGroupReadService;

		public ListingOwnershipQuery(IListingReadService listingReadService, IListingGroupReadService listingGroupReadService)
		{
			_listingReadService = listingReadService;
			_listingGroupReadService = listingGroupReadService;
		}


        public async Task<Guid> GetSellerUserIdByListingGroupId(Guid listingGroupId)
        {
            ListingGroup? listingGroup = await _listingGroupReadService.GetListingGroupByIdAsync(listingGroupId);
            if (listingGroup == null)
            {
                throw new KeyNotFoundException("ListingGroup not found");
            }
            return listingGroup.SellerUserId;
        }

        public async Task<Guid> GetSellerUserIdByListingId(Guid listingId)
		{
			Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}
			return await GetSellerUserIdByListingGroupId(listing.ListingGroupId);
		}
	}
}
