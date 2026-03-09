using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;


namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingSales
{
    internal class ListingSaleReadService : IListingSaleReadService
	{
		private readonly IListingSaleRepository _listingSaleRepository;

		public ListingSaleReadService(IListingSaleRepository listingSaleRepository)
		{
			_listingSaleRepository = listingSaleRepository;
		}

		public async Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId)
		{
			return await _listingSaleRepository.GetListingSaleByIdAsync(listingSaleId);
		}

		public async Task<ListingSale?> GetListingSaleByListingId(Guid listingId)
		{
			return await _listingSaleRepository.GetListingSaleByListingIdAsync(listingId);
		}

		public async Task<ListingSale?> GetListingSaleByQueueEntryId(Guid queueEntryId)
		{
			return await _listingSaleRepository.GetListingSaleByListingQueueEntryIdAsync(queueEntryId);
		}

		public async Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId)
		{
			return await _listingSaleRepository.GetListingSalesByBuyerUserIdAsync(buyerUserId);
		}

		public async Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate)
		{
			return await _listingSaleRepository.GetListingSalesByDateRange(startDate, endDate);
		}

		public async Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId)
		{
			return await _listingSaleRepository.GetListingSalesBySellerUserIdAsync(sellerUserId);
		}

        public async Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId)
        {
            return await _listingSaleRepository.GetListingSalesByUserIdAsync(userId);
        }
    }
}
