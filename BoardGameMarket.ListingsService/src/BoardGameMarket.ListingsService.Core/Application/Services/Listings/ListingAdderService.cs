using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingAdderService : IListingAdderService
	{
		private readonly IListingRepository _listingRepository;
		private readonly ILogger<ListingAdderService> _logger;

		public ListingAdderService(IListingRepository listingRepository, ILogger<ListingAdderService> logger)
		{
			_listingRepository = listingRepository;
			_logger = logger;
		}

		private Listing BuildListing(ListingAddRequest listingAddRequest)
		{
			Listing listing = listingAddRequest.ToListing();
			listing.Id = Guid.NewGuid();
			listing.CreatedAt = DateTime.UtcNow;
			listing.ModifiedAt = DateTime.UtcNow;

			return listing;
		}

		public async Task<Listing?> AddListingAsync(Listing listingToAdd)
		{
			listingToAdd.Id = Guid.NewGuid();
			listingToAdd.CreatedAt = DateTime.UtcNow;
			listingToAdd.ModifiedAt = DateTime.UtcNow;
			return await _listingRepository.AddListingAsync(listingToAdd);
		}

		public async Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listingsToAdd)
		{

			foreach (Listing listing in listingsToAdd)
			{
				listing.Id = Guid.NewGuid();
				listing.CreatedAt = DateTime.UtcNow;
				listing.ModifiedAt = DateTime.UtcNow;
			}
			_logger.LogInformation($"{nameof(ListingAdderService)}:{nameof(AddListingsAsync)}- adding listings");

			return await _listingRepository.AddListingsAsync(listingsToAdd);
		}
	}
}
