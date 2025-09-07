using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	public class ListingAdderService : IListingAdderService
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

		public async Task<Listing?> AddListingAsync(ListingAddRequest? listingAddRequest)
		{
			ArgumentNullException.ThrowIfNull(listingAddRequest, nameof(listingAddRequest));

			_logger.LogInformation($"{nameof(ListingAdderService)}:{nameof(AddListingAsync)}- adding listing");

			Listing listingToAdd = BuildListing(listingAddRequest);

			return await _listingRepository.AddListingAsync(listingToAdd);
		}

		public async Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<ListingAddRequest>? listingAddRequests)
		{
			if (listingAddRequests == null)
			{
				return Enumerable.Empty<Listing>();
			}
			_logger.LogInformation($"{nameof(ListingAdderService)}:{nameof(AddListingsAsync)}- adding listings");

			IEnumerable<Listing> listings = listingAddRequests.Select(BuildListing);

			return await _listingRepository.AddListingsAsync(listings);
		}
	}
}
