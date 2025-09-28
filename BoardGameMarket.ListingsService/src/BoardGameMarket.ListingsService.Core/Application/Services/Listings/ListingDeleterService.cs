using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingDeleterService : IListingDeleterService
	{
		private readonly IListingRepository _listingRepository;

		public ListingDeleterService(IListingRepository listingRepository)
		{
			_listingRepository = listingRepository;
		}

		public async Task<bool> DeleteListingAsync(Guid listingId)
		{
			return await _listingRepository.DeleteListingAsync(listingId);
		}

		public async Task DeleteListingsByListingGroupId(Guid listingGroupId)
		{
			await _listingRepository.DeleteListingsByListingGroupIdAsync(listingGroupId);
		}
	}
}
