using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups
{
	public interface IListingGroupWriteService
	{
		Task<ListingGroup?> AddListingGroup(ListingGroup listingGroupToAdd);
		Task<bool> DeleteListingGroup(Guid listingGroupId);
		Task<ListingGroup?> UpdateListingGroupAsync(ListingGroup listingGroupToUpdate);
	}
}
