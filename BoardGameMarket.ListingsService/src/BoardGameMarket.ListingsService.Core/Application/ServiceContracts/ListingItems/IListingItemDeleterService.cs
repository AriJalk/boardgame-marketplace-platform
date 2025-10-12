using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems
{
	public interface IListingItemDeleterService
	{
		Task<bool> DeleteListingItemAsync(Guid listingItemId);
		Task DeleteListingItemsByListingIdAsync(Guid listingId);
	}
}
