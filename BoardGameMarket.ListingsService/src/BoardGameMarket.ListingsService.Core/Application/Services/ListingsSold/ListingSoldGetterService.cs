using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;


namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold
{
	public class ListingSoldGetterService : IListingSoldGetterService
	{
		private readonly IListingSoldRepository _listingSoldRepository;
		private readonly ILogger<ListingSoldGetterService> _logger;

		public ListingSoldGetterService(IListingSoldRepository listingSoldRepository, ILogger<ListingSoldGetterService> logger)
		{
			_listingSoldRepository = listingSoldRepository;
			_logger = logger;
		}

		public async Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingSoldByIdAsync)}- Get listing sold by id {listingSoldId}");

			return await _listingSoldRepository.GetListingSoldByIdAsync(listingSoldId);
		}

		public async Task<ListingSold?> GetListingSoldByListingId(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingSoldByListingId)}- Get listing sold by listing id {listingId}");

			return await _listingSoldRepository.GetListingSoldByListingIdAsync(listingId);
		}

		public async Task<ListingSold?> GetListingSoldByQueueEntryId(Guid queueEntryId)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingSoldByQueueEntryId)}- Get listing sold by queue entry id {queueEntryId}");

			return await _listingSoldRepository.GetListingSoldByListingQueueEntryIdAsync(queueEntryId);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingsSoldByBuyerUserIdAsync)}- Get listings sold by buyer id {buyerUserId}");

			return await _listingSoldRepository.GetListingsSoldByBuyerUserIdAsync(buyerUserId);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingsSoldByDateRange)}- Get listings sold by date range {startDate}-{endDate}");

			return await _listingSoldRepository.GetListingsSoldByDateRange(startDate, endDate);
		}

		public async Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
		{
			_logger.LogInformation($"{nameof(ListingSoldGetterService)}:{nameof(GetListingsSoldBySellerUserIdAsync)}- Get listings sold by seller id {sellerUserId}");

			return await _listingSoldRepository.GetListingsSoldBySellerUserIdAsync(sellerUserId);
		}
	}
}
