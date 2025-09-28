using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingAdderService : IListingAdderService
	{
		private readonly IListingRepository _listingRepository;

		public ListingAdderService(IListingRepository listingRepository)
		{
			_listingRepository = listingRepository;
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

			return await _listingRepository.AddListingsAsync(listingsToAdd);
		}
	}
}
