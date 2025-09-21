using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups
{
	public interface IListingGroupAdderService
	{
		Task<ListingGroup?> AddListingGroup(ListingGroup listingGroupToAdd);
	}
}
