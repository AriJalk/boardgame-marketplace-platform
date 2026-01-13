using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold
{
	internal class ListingSoldAdderService : IListingSoldAdderService
	{
		private readonly IListingSoldRepository _listingSoldRepository;

		public ListingSoldAdderService(IListingSoldRepository listingSoldRepository)
		{
			_listingSoldRepository = listingSoldRepository;
		}

		public async Task<ListingSold?> AddListingSoldAsync(ListingSold listingSoldToAdd)
		{
			listingSoldToAdd.Id = Guid.NewGuid();
			listingSoldToAdd.CompletedAt = DateTime.UtcNow;
			return await _listingSoldRepository.AddListingSoldAsync(listingSoldToAdd);
		}
	}
}
