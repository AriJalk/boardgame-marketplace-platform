using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;


namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
    internal class ListingSoldReadService : IListingSoldReadService
	{
		private readonly IListingSoldRepository _listingSoldRepository;

		public ListingSoldReadService(IListingSoldRepository listingSoldRepository)
		{
			_listingSoldRepository = listingSoldRepository;
		}

		public async Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId)
		{
			return await _listingSoldRepository.GetListingSoldByIdAsync(listingSoldId);
		}

		public async Task<ListingSold?> GetListingSoldByListingId(Guid listingId)
		{
			return await _listingSoldRepository.GetListingSoldByListingIdAsync(listingId);
		}

		public async Task<ListingSold?> GetListingSoldByQueueEntryId(Guid queueEntryId)
		{
			return await _listingSoldRepository.GetListingSoldByListingQueueEntryIdAsync(queueEntryId);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
		{
			return await _listingSoldRepository.GetListingsSoldByBuyerUserIdAsync(buyerUserId);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
		{
			return await _listingSoldRepository.GetListingsSoldByDateRange(startDate, endDate);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
		{
			return await _listingSoldRepository.GetListingsSoldBySellerUserIdAsync(sellerUserId);
		}

        public async Task<IEnumerable<ListingSold>> GetListingsSoldByUserIdAsync(Guid userId)
        {
            return await _listingSoldRepository.GetListingsSoldByUserIdAsync(userId);
        }
    }
}
