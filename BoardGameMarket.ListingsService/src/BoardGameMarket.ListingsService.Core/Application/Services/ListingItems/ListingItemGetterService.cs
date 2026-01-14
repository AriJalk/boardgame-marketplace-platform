using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingItems
{
	internal class ListingItemGetterService : IListingItemGetterService
	{
		private readonly IListingItemRepository _listingItemRepository;

		public ListingItemGetterService(IListingItemRepository listingItemRepository)
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
