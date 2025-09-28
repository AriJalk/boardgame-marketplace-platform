using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingUpdaterService : IListingUpdaterService
	{
		private readonly IListingRepository _listingRepository;

		public ListingUpdaterService(IListingRepository listingRepository)
		{
			_listingRepository = listingRepository;
		}

		public async Task<Listing?> UpdateListingAsync(Listing listingToUpdate)
		{
			listingToUpdate.ModifiedAt = DateTime.Now;
			return await _listingRepository.UpdateListingAsync(listingToUpdate);
		}
	}
}
