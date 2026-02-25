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

		public async Task AddListingAsync(Listing listingToAdd)
		{
			int rows = await _listingRepository.AddListingAsync(listingToAdd);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task AddListingsAsync(IEnumerable<Listing> listingsToAdd)
		{
			List<Listing> listings = listingsToAdd.ToList();
			int rows = await _listingRepository.AddListingsAsync(listings);
			if (rows != listings.Count)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task DeleteListingAsync(Guid listingId)
		{
			int rows = await _listingRepository.DeleteListingAsync(listingId);
		}

		public async Task DeleteListingsByListingGroupId(Guid listingGroupId)
		{
			int rows = await _listingRepository.DeleteListingsByListingGroupIdAsync(listingGroupId);
		}

		public async Task UpdateListingAsync(Listing listingToUpdate)
		{
			int rows = await _listingRepository.UpdateListingAsync(listingToUpdate);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}
	}
}
