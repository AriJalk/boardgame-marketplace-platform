using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
	internal class ListingSoldWriteService : IListingSoldWriteService
	{
		private readonly IListingSoldRepository _listingSoldRepository;

		public ListingSoldWriteService(IListingSoldRepository listingSoldRepository)
		{
			_listingSoldRepository = listingSoldRepository;
		}

		public async Task AddListingSoldAsync(ListingSold listingSoldToAdd)
		{
			int rows = await _listingSoldRepository.AddListingSoldAsync(listingSoldToAdd);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task DeleteListingSoldAsync(Guid listingSoldId)
		{
			int rows = await _listingSoldRepository.DeleteListingSoldAsync(listingSoldId);
		}
	}
}
