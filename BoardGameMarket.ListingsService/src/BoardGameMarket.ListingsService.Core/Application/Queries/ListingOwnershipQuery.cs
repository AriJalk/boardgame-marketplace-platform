using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Queries
{
	public class ListingOwnershipQuery
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingGroupGetterService _listingGroupGetterService;

		public ListingOwnershipQuery(IListingGetterService listingGetterService, IListingGroupGetterService listingGroupGetterService)
		{
			_listingGetterService = listingGetterService;
			_listingGroupGetterService = listingGroupGetterService;
		}

		public async Task<Guid> GetSellerUserIdByListingId(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException("Listing not found");
			}
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listing.ListingGroupId);
			if (listingGroup == null)
			{
				throw new KeyNotFoundException();
			}
			return listingGroup.SellerUserId;
		}
	}
}
