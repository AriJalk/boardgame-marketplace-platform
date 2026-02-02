using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.Access.Listings
{
	internal class ListingWriteService : IListingWriteService
	{
		private readonly IListingRepository _listingRepository;

		public ListingWriteService(IListingRepository listingRepository)
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

		public async Task<bool> DeleteListingAsync(Guid listingId)
		{
			return await _listingRepository.DeleteListingAsync(listingId);
		}

		public async Task DeleteListingsByListingGroupId(Guid listingGroupId)
		{
			await _listingRepository.DeleteListingsByListingGroupIdAsync(listingGroupId);
		}

		public async Task<Listing?> UpdateListingAsync(Listing listingToUpdate)
		{
			listingToUpdate.ModifiedAt = DateTime.Now;
			return await _listingRepository.UpdateListingAsync(listingToUpdate);
		}
	}
}
