using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingSales
{
	internal class ListingSaleWriteService : IListingSaleWriteService
	{
		private readonly IListingSaleRepository _listingSaleRepository;

		public ListingSaleWriteService(IListingSaleRepository listingSaleRepository)
		{
			_listingSaleRepository = listingSaleRepository;
		}

		public async Task AddListingSaleAsync(ListingSale listingSaleToAdd)
		{
			int rows = await _listingSaleRepository.AddListingSaleAsync(listingSaleToAdd);
			if (rows == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task DeleteListingSaleAsync(Guid listingSaleId)
		{
			int rows = await _listingSaleRepository.DeleteListingSaleAsync(listingSaleId);
		}
	}
}
