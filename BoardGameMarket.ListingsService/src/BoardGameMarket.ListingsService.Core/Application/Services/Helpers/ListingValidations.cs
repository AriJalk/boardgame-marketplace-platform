using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Helpers
{
	internal class ListingValidations
	{
		private IListingSoldGetterService _listingSoldGetterService;

		public ListingValidations(IListingSoldGetterService listingSoldGetterService)
		{
			_listingSoldGetterService = listingSoldGetterService;
		}

		public async Task ValidateListingNotSold(Listing listing)
		{
			if (await _listingSoldGetterService.GetListingSoldByListingId(listing.Id) != null || listing.Status == ListingStatus.Sold)
			{
				throw new InvalidOperationException("Listing already sold");
			}
		}
	}
}
