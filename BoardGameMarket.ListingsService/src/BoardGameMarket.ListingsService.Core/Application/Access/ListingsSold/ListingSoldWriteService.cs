using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
	internal class ListingSoldWriteService : IListingSoldWriteService
	{
		private readonly IListingSoldRepository _listingSoldRepository;

		public ListingSoldWriteService(IListingSoldRepository listingSoldRepository)
		{
			_listingSoldRepository = listingSoldRepository;
		}

		public async Task<ListingSold?> AddListingSoldAsync(ListingSold listingSoldToAdd)
		{
			listingSoldToAdd.Id = Guid.NewGuid();
			listingSoldToAdd.CompletedAt = DateTime.UtcNow;
			return await _listingSoldRepository.AddListingSoldAsync(listingSoldToAdd);
		}

		public async Task<bool> DeleteListingSoldAsync(Guid listingSoldId)
		{
			return await _listingSoldRepository.DeleteListingSoldAsync(listingSoldId); ;
		}
	}
}
