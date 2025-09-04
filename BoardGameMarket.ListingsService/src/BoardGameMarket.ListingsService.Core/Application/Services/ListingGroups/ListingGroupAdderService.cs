using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupAdderService : IListingGroupAdderService
	{
		public Task<ListingGroup?> AddListingGroup(ListingGroupAddRequest? listingGroupAddRequest)
		{
			throw new NotImplementedException();
		}
	}
}
