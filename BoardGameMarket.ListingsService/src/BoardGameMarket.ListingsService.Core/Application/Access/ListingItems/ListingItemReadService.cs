using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingItems
{
	internal class ListingItemReadService : IListingItemReadService
	{
		private readonly IListingItemRepository _listingItemRepository;

		public ListingItemReadService(IListingItemRepository listingItemRepository)
		{
			_listingItemRepository = listingItemRepository;
		}

		public async Task<ListingItem?> GetListingItemById(Guid listingItemId)
		{
			return await _listingItemRepository.GetListingItemByIdAsync(listingItemId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
		{
			return await _listingItemRepository.GetListingItemsByCatalogIdAsync(catalogId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdAsync(Guid listingId)
		{
			return await _listingItemRepository.GetListingsItemsByListingIdAsync(listingId);
		}

		public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			
			return await _listingItemRepository.GetListingItemsByListingIdsAsync(listingIds);
		}
	}
}
