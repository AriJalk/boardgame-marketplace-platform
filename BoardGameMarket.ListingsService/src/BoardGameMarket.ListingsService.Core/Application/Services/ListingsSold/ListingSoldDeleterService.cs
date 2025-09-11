using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold
{
	internal class ListingSoldDeleterService : IListingSoldDeleterService
	{
		private readonly IListingSoldRepository _listingSoldRepository;
		private readonly ILogger<ListingSoldDeleterService> _logger;

		public ListingSoldDeleterService(IListingSoldRepository listingSoldRepository, ILogger<ListingSoldDeleterService> logger)
		{
			_listingSoldRepository = listingSoldRepository;
			_logger = logger;
		}

		public async Task<bool> DeleteListingSoldAsync(Guid listingSoldId)
		{
			_logger.LogInformation($"{nameof(ListingSoldDeleterService)}:{nameof(DeleteListingSoldAsync)}- Deleting listing sold {listingSoldId}");

			return await _listingSoldRepository.DeleteListingSoldAsync(listingSoldId); ;
		}
	}
}
