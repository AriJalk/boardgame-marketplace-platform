using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	public class ListingDeleterService : IListingDeleterService
	{
		private readonly IListingRepository _listingRepository;
		private readonly ILogger<ListingDeleterService> _logger;

		public ListingDeleterService(IListingRepository listingRepository, ILogger<ListingDeleterService> logger)
		{
			_listingRepository = listingRepository;
			_logger = logger;
		}

		public async Task<bool> DeleteListingAsync(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingDeleterService)}:{nameof(DeleteListingAsync)}- Deleting listing {listingId}");

			return await _listingRepository.DeleteListing(listingId);

		}
	}
}
