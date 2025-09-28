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
		Task<Listing?> AddListingAsync(Listing listing);
		Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listings);
		Task<Listing?> UpdateListingAsync(Listing listing);
		Task<bool> DeleteListingAsync(Guid listingId);
		Task DeleteListingsByListingGroupIdAsync(Guid listingGroupId);
	}
}
