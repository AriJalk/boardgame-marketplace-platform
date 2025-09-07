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
	public class ListingUpdaterService : IListingUpdaterService
	{
		private readonly IListingRepository _listingRepository;
		private readonly ILogger<ListingUpdaterService> _logger;

		public ListingUpdaterService(IListingRepository listingRepository, ILogger<ListingUpdaterService> logger)
		{
			_listingRepository = listingRepository;
			_logger = logger;
		}

		public async Task<Listing?> UpdateListingAsync(ListingUpdateRequest? listingUpdateRequest)
		{
			ArgumentNullException.ThrowIfNull(listingUpdateRequest, nameof(listingUpdateRequest));

			Listing? listingToUpdate = await _listingRepository.GetListingByIdAsync(listingUpdateRequest.Id);
			if (listingToUpdate == null)
			{
				return null;
			}

			if (listingToUpdate.ListingGroupId != listingUpdateRequest.ListingGroupId)
			{
				throw new ArgumentException("Mismatch update info");
			}

			bool isChanged = false;

			if (listingToUpdate.Price != listingUpdateRequest.Price)
			{
				listingToUpdate.Price = listingUpdateRequest.Price;
				isChanged = true;
			}

			if (listingToUpdate.Details != listingUpdateRequest.Details)
			{
				listingToUpdate.Details = listingUpdateRequest.Details;
				isChanged = true;
			}

			if (isChanged)
			{
				_logger.LogInformation($"{nameof(ListingUpdaterService)}:{nameof(UpdateListingAsync)}- update listing {listingToUpdate.Id}");
				listingToUpdate.ModifiedAt = DateTime.UtcNow;
				return await _listingRepository.UpdateListingAsync(listingToUpdate);
			}

			return null;
		}
	}
}
