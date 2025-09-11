using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups
{
	internal interface IListingGroupDeleterService
	{
		Task<bool> DeleteListingGroup(Guid listingGroupId);
	}
}
