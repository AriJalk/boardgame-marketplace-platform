using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
	public interface IListingRepository
	{
		Task<Listing?> GetListingByIdAsync(Guid listingId);
		Task<IEnumerable<Listing>> GetListingsByGroupIdAsync(Guid groupId);
		Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds);
		Task<IEnumerable<Listing>> GetListings();
		Task<int> AddListingAsync(Listing listing);
		Task<int> AddListingsAsync(IEnumerable<Listing> listings);
		Task<int> UpdateListingAsync(Listing listing);
		Task<int> UpdateListingsAsync(IEnumerable<Listing> listings);
		Task<int> DeleteListingAsync(Guid listingId);
		Task<int> DeleteListingsByListingGroupIdAsync(Guid listingGroupId);
	}
}
