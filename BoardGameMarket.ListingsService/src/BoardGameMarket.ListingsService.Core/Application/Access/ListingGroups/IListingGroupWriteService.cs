using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups
{
	public interface IListingGroupWriteService
	{
		Task AddListingGroup(ListingGroup listingGroupToAdd);
		Task DeleteListingGroup(Guid listingGroupId);
		Task UpdateListingGroupAsync(ListingGroup listingGroupToUpdate);
	}
}
