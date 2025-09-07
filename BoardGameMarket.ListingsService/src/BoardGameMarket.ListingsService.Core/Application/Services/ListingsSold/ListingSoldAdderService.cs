using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold
{
	public class ListingSoldAdderService : IListingSoldAdderService
	{
		private readonly IListingSoldRepository _listingSoldRepository;
		private readonly ILogger<ListingSoldAdderService> _logger;

		public ListingSoldAdderService(IListingSoldRepository listingSoldRepository, ILogger<ListingSoldAdderService> logger)
		{
			_listingSoldRepository = listingSoldRepository;
			_logger = logger;
		}

		public async Task<ListingSold?> AddListingSoldAsync(ListingSoldAddRequest? listingSoldAddRequest)
		{
			ArgumentNullException.ThrowIfNull(listingSoldAddRequest, nameof(listingSoldAddRequest));

			_logger.LogInformation($"{nameof(ListingSoldAdderService)}:{nameof(AddListingSoldAsync)}- adding listing sold request");
			ListingSold listingSoldToAdd = listingSoldAddRequest.ToListingSold();
			listingSoldToAdd.Id = Guid.NewGuid();
			listingSoldToAdd.CompletedAt = DateTime.UtcNow;

			return await _listingSoldRepository.AddListingSoldAsync(listingSoldToAdd);
		}
	}
}
