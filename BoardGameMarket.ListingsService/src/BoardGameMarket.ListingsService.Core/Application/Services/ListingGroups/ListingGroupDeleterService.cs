using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupDeleterService : IListingGroupDeleterService
	{
		public Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			throw new NotImplementedException();
		}
	}
}
