using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingGroupRepository
	{
		Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId);
		Task<ListingGroup?> GetListingsGroupByListingIdAsync(Guid listingId);
		Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId);
		Task<ListingGroup> AddListingGroupAsync(ListingGroup listingGroup);
		Task<bool?> DeleteListingGroup(Guid listingGroupId);
	}
}
