using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups
{
	public interface IListingGroupAdderService
	{
		Task<ListingGroup?> AddListingGroup(ListingGroup listingGroupToAdd);
	}
}
